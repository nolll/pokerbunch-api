using Api.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using Api.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Api;
using Api.Bootstrapping;
using Api.Extensions.Swagger;
using Api.Middleware;
using Api.Routes;
using Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT");
var settings = builder.Configuration.Get<AppSettings>() ?? new AppSettings();
var isDev = builder.Environment.IsDevelopment();
var isProd = !isDev;

if (!string.IsNullOrEmpty(port))
    builder.WebHost.UseUrls("http://*:" + port);

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

var connectionString = GetConnectionString(builder.Configuration);
builder.Services.AddDbContext<PokerBunchDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddServices(settings, builder.Configuration, connectionString);

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

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Poker Bunch Api";
        document.Info.Description = "For access to protected endpoints, you will need a token from the Login endpoints.";
        return Task.CompletedTask;
    });
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

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
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/openapi/v1.json", "Version 1");
});

app.UseAuthentication();
app.UseAuthorization();
app.Map();
app.UseMvc();

app.Run();
return;

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

static bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters param)
{
    if (expires != null)
        return expires > DateTime.UtcNow;

    return false;
}

public partial class Program { } // Needed for integration tests