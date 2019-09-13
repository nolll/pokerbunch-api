using System;
using System.IO;
using System.Reflection;
using System.Text;
using Api.Extensions;
using Api.Settings;
using Api.Urls.ApiUrls;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
            AddCors();
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
        }

        private void AddMvc()
        {
            _services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        private void AddCors()
        {
            _services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
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
                c.SwaggerDoc("v1", new Info { Title = "Poker Bunch Api", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}