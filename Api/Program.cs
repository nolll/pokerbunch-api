using Api.Extensions;
using Api.Settings;
using Api.Urls.ApiUrls;
using Core;
using Core.Cache;
using Core.Repositories;
using Core.Services;
using Core.UseCases;
using Infrastructure.Cache;
using Infrastructure.Email;
using Infrastructure.Sql;
using Infrastructure.Sql.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Text;
using Api.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Api.Extensions.Swagger;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using Api;
using Api.Controllers;
using Api.Middleware;
using Api.Routes;

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT");
var settings = builder.Configuration.Get<AppSettings>() ?? new AppSettings();
var isDev = builder.Environment.IsDevelopment();
var isProd = !isDev;

if (!string.IsNullOrEmpty(port))
    builder.WebHost.UseUrls("http://*:" + port);

var connectionString = GetConnectionString(builder.Configuration);

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.AddLogging(logging =>
{
    if (settings.Logging.Loggers.Debug)
        logging.AddDebug();

    if (settings.Logging.Loggers.Console)
        logging.AddConsole();

    logging.SetMinimumLevel(settings.Logging.LogLevel.Default);
});

builder.Services.AddSingleton(settings);
builder.Services.AddSingleton<ISettings>(new Settings(settings.InvitationSecret));
builder.Services.AddSingleton(new UrlProvider(settings.Urls.Api, settings.Urls.Site));

builder.Services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
builder.Services.AddSingleton<ICache, Cache>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IBunchRepository, BunchRepository>();
builder.Services.AddSingleton<ICashgameRepository, CashgameRepository>();
builder.Services.AddSingleton<IEventRepository, EventRepository>();
builder.Services.AddSingleton<ILocationRepository, LocationRepository>();
builder.Services.AddSingleton<IPlayerRepository, PlayerRepository>();
builder.Services.AddSingleton(GetEmailSender(builder.Configuration));
builder.Services.AddSingleton<IDb>(new PostgresDb(connectionString));
builder.Services.AddSingleton<IRandomizer, Randomizer>();
builder.Services.AddSingleton<IInvitationCodeCreator, InvitationCodeCreator>();

// Admin
builder.Services.AddSingleton<ClearCache>();
builder.Services.AddSingleton<TestEmail>();
builder.Services.AddSingleton<RequireAppsettingsAccess>();

// Auth
builder.Services.AddSingleton<Login>();

// User
builder.Services.AddSingleton<UserDetails>();
builder.Services.AddSingleton<UserList>();
builder.Services.AddSingleton<EditUser>();
builder.Services.AddSingleton<AddUser>();
builder.Services.AddSingleton<ChangePassword>();
builder.Services.AddSingleton<ResetPassword>();

// Bunch
builder.Services.AddSingleton<GetBunchList>();
builder.Services.AddSingleton<GetBunchListForUser>();
builder.Services.AddSingleton<GetBunch>();
builder.Services.AddSingleton<AddBunch>();
builder.Services.AddSingleton<EditBunch>();

// Events
builder.Services.AddSingleton<EventDetails>();
builder.Services.AddSingleton<EventList>();
builder.Services.AddSingleton<AddEvent>();

// Locations
builder.Services.AddSingleton<GetLocationList>();
builder.Services.AddSingleton<GetLocation>();
builder.Services.AddSingleton<AddLocation>();

// Cashgame
builder.Services.AddSingleton<CashgameList>();
builder.Services.AddSingleton<EventCashgameList>();
builder.Services.AddSingleton<PlayerCashgameList>();
builder.Services.AddSingleton<CurrentCashgames>();
builder.Services.AddSingleton<CashgameDetails>();
builder.Services.AddSingleton<Buyin>();
builder.Services.AddSingleton<Report>();
builder.Services.AddSingleton<Cashout>();
builder.Services.AddSingleton<AddCashgame>();
builder.Services.AddSingleton<EditCashgame>();
builder.Services.AddSingleton<DeleteCashgame>();
builder.Services.AddSingleton<EditCheckpoint>();
builder.Services.AddSingleton<DeleteCheckpoint>();

// Player
builder.Services.AddSingleton<GetPlayer>();
builder.Services.AddSingleton<GetPlayerList>();
builder.Services.AddSingleton<AddPlayer>();
builder.Services.AddSingleton<DeletePlayer>();
builder.Services.AddSingleton<InvitePlayer>();
builder.Services.AddSingleton<JoinBunch>();

builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder => policyBuilder
        .SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthSecretProvider.GetSecret(settings.Auth.Secret))),
            ValidateIssuer = false,
            ValidateAudience = false,
            LifetimeValidator = CustomLifetimeValidator
        };
    });

builder.Services.AddOpenApi();
// builder.Services.AddSwaggerGen(c =>
// {
//     var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
//     var xmlFile = $"{assemblyName}.xml";
//     var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//     c.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Title = "Poker Bunch Api", 
//         Description = "For access to protected endpoints, you will need a token from the [Login endpoints](#operations-User-post_login).",
//         Version = "v1"
//     });
//     c.IncludeXmlComments(xmlPath);
//     c.OperationFilter<AuthorizeCheckOperationFilter>();
//     c.CustomSchemaIds(SwaggerSchema.GetSwaggerTypeName);
//     c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         In = ParameterLocation.Header,
//         Description = "Token",
//         Name = "Authorization",
//         Type = SecuritySchemeType.Http,
//         BearerFormat = "JWT",
//         Scheme = "Bearer"
//     });
// });

var app = builder.Build();

app.UseCors("CorsPolicy");
app.UseResponseCompression();

if (isProd)
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseMiddleware<SecurityHeadersMiddleware>();

if (settings.Error.DetailedErrors)
{
    app.UseDeveloperExceptionPage();
}
else
{
    const string errorUrl = $"/{ApiRoutes.Error}";
    app.UseStatusCodePagesWithReExecute(errorUrl);
    app.UseExceptionHandler(errorUrl);
    app.UseMiddleware<ExceptionLoggingMiddleware>();
}

app.MapOpenApi();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/openapi/v1.json", "Version 1"); });

app.UseAuthentication();
app.UseAuthorization();
app.MapAdminEndpoints();
app.UseMvc();

app.Run();

static bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters param)
{
    if (expires != null)
        return expires > DateTime.UtcNow;

    return false;
}

static string GetConnectionString(IConfiguration configuration)
{
    var databaseUrl = configuration.GetValue<string>("DATABASE_URL");
    if (string.IsNullOrEmpty(databaseUrl))
        return "";

    var databaseUri = new Uri(databaseUrl);
    var userInfo = databaseUri.UserInfo.Split(':');

    var connectionStringBuilder = new NpgsqlConnectionStringBuilder
    {
        Host = databaseUri.Host,
        Port = databaseUri.Port,
        Username = userInfo[0],
        Password = userInfo[1],
        Database = databaseUri.LocalPath.TrimStart('/')
    };

    return connectionStringBuilder.ToString();
}

static IEmailSender GetEmailSender(IConfiguration configuration)
{
    var host = configuration.GetValue<string>("SMTP_SERVER") ?? "localhost";
    var strPort = configuration.GetValue<string>("SMTP_PORT");
    var port = strPort != null ? int.Parse(strPort) : 25;
    var login = configuration.GetValue<string>("SMTP_LOGIN");
    var password = configuration.GetValue<string>("SMTP_PASSWORD");
    return new SmtpEmailSender(host, port, login, password);
}

public partial class Program { } // Needed for integration tests