﻿using System.Text;
using Api.Extensions;
using Api.Urls.ApiUrls;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Api
{
    public class Startup
    {
        private readonly Settings _settings;

        public Startup(IConfiguration configuration)
        {
            _settings = new Settings(configuration.Get<AppSettings>());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            AddDependencies(services);
            AddMvc(services);
            AddCors(services);
            AddAuthorization(services);
            AddAuthentication(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseMvc();
        }

        private void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(x =>
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.AuthSecret)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        private static void AddAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserPolicy", policy => policy.Requirements.Add(new CustomAuthRequirement()));
            });
        }

        private static void AddMvc(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        private static void AddCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }

        private void AddDependencies(IServiceCollection services)
        {
            services.AddSingleton(_settings);
            services.AddSingleton(new UrlProvider(_settings.ApiHost, _settings.SiteHost));
            services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();
        }
    }
}
