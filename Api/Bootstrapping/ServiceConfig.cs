using System;
using System.IO;
using System.Reflection;
using System.Text;
using Api.Extensions;
using Api.Extensions.Swagger;
using Api.Settings;
using Api.Urls.ApiUrls;
using Core.Cache;
using Core.Repositories;
using Core.Services;
using Core.UseCases;
using Infrastructure.Cache;
using Infrastructure.Email;
using Infrastructure.Sql;
using Infrastructure.Sql.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Api.Bootstrapping
{
    public class ServiceConfig
    {
        private readonly AppSettings _settings;
        private readonly IServiceCollection _services;

        public ServiceConfig(AppSettings settings, IServiceCollection services)
        {
            _settings = settings;
            _services = services;
        }

        public void Configure()
        {
            AddCompression();
            AddLogging();
            AddDependencies();
            AddMvc();
            AddAuthorization();
            AddAuthentication();
            AddSwagger();
        }

        private void AddCompression()
        {
            _services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });
        }

        private void AddLogging()
        {
            _services.AddLogging(logging =>
            {
                if (_settings.Logging.Loggers.Debug)
                    logging.AddDebug();

                if (_settings.Logging.Loggers.Console)
                    logging.AddConsole();

                logging.SetMinimumLevel(_settings.Logging.LogLevel);
            });
        }

        private void AddDependencies()
        {
            _services.AddSingleton(_settings);
            _services.AddSingleton(new UrlProvider(_settings.Urls.Api, _settings.Urls.Site));

            _services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();
            _services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
            _services.AddSingleton<ICacheContainer, CacheContainer>();
            _services.AddSingleton<SqlServerStorageProvider>();
            _services.AddSingleton<IUserRepository, UserRepository>();
            _services.AddSingleton<IBunchRepository, BunchRepository>();
            _services.AddSingleton<ICashgameRepository, CashgameRepository>();
            _services.AddSingleton<IEventRepository, EventRepository>();
            _services.AddSingleton<ILocationRepository, LocationRepository>();
            _services.AddSingleton<IPlayerRepository, PlayerRepository>();
            _services.AddSingleton<ITimezoneRepository, TimezoneRepository>();
            _services.AddSingleton(GetEmailSender());
            _services.AddSingleton(new SqlServerStorageProvider(_settings.Sql.ConnectionString));
            _services.AddSingleton<IRandomizer, Randomizer>();

            // Admin
            _services.AddSingleton<ClearCache>();
            _services.AddSingleton<TestEmail>();
            _services.AddSingleton<EnsureAdmin>();

            // Misc
            _services.AddSingleton<GetTimezoneList>();

            // Auth
            _services.AddSingleton<Login>();

            // User
            _services.AddSingleton<UserDetails>();
            _services.AddSingleton<UserList>();
            _services.AddSingleton<EditUser>();
            _services.AddSingleton<AddUser>();
            _services.AddSingleton<ChangePassword>();
            _services.AddSingleton<ResetPassword>();

            // Bunch
            _services.AddSingleton<GetBunchList>();
            _services.AddSingleton<GetBunch>();
            _services.AddSingleton<AddBunch>();
            _services.AddSingleton<EditBunch>();

            // Events
            _services.AddSingleton<EventDetails>();
            _services.AddSingleton<EventList>();
            _services.AddSingleton<AddEvent>();

            // Locations
            _services.AddSingleton<GetLocationList>();
            _services.AddSingleton<GetLocation>();
            _services.AddSingleton<AddLocation>();

            // Cashgame
            _services.AddSingleton<CashgameList>();
            _services.AddSingleton<CashgameYearList>();
            _services.AddSingleton<EventCashgameList>();
            _services.AddSingleton<PlayerCashgameList>();
            _services.AddSingleton<CurrentCashgames>();
            _services.AddSingleton<CashgameDetails>();
            _services.AddSingleton<Buyin>();
            _services.AddSingleton<Report>();
            _services.AddSingleton<Cashout>();
            _services.AddSingleton<AddCashgame>();
            _services.AddSingleton<EditCashgame>();
            _services.AddSingleton<DeleteCashgame>();
            _services.AddSingleton<EditCheckpoint>();
            _services.AddSingleton<DeleteCheckpoint>();

            // Player
            _services.AddSingleton<GetPlayer>();
            _services.AddSingleton<GetPlayerList>();
            _services.AddSingleton<AddPlayer>();
            _services.AddSingleton<DeletePlayer>();
            _services.AddSingleton<InvitePlayer>();
            _services.AddSingleton<JoinBunch>();
        }

        private IEmailSender GetEmailSender()
        {
            if (_settings.Email.Provider == EmailProvider.SendGrid)
                return new SendGridEmailSender(_settings.Email.SendGrid.Key);
            return new SmtpEmailSender(_settings.Email.Smtp.Host);
        }

        private void AddMvc()
        {
            _services
                .AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                });
        }

        private void AddAuthentication()
        {
            _services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.Auth.Secret)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        LifetimeValidator = CustomLifetimeValidator
                    };
                });
        }

        private bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters @param)
        {
            if (expires != null)
                return expires > DateTime.UtcNow;

            return false;
        }

        private void AddAuthorization()
        {
            _services.AddAuthorization(options =>
            {
                options.AddPolicy("UserPolicy", policy => policy.Requirements.Add(new CustomAuthRequirement()));
            });
        }

        private void AddSwagger()
        {
            _services.AddSwaggerGen(c =>
            {
                var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                var xmlFile = $"{assemblyName}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Poker Bunch Api", Version = "v1" });
                c.IncludeXmlComments(xmlPath);
                c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });
        }
    }
}