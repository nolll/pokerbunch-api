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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace Api
{
    public class Startup
    {
        //        private static void ConfigFormatters(HttpConfiguration config)
        //        {
        //            var jsonFormatter = new JsonMediaTypeFormatter();
        //            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        //            jsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        //            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));

        //            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));

        //            config.Formatters.Clear();
        //            config.Formatters.Add(jsonFormatter);
        //        }

        //        private void ConfigureErrorHandler(HttpConfiguration config)
        //        {
        //            config.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());
        //            config.Services.Replace(typeof(IHttpControllerSelector), new HttpNotFoundAwareDefaultHttpControllerSelector(config));
        //            config.Services.Replace(typeof(IHttpActionSelector), new HttpNotFoundAwareControllerActionSelector());
        //        }

        //        private void ConfigureErrorLogger(HttpConfiguration config)
        //        {
        //            config.Services.Add(typeof(IExceptionLogger), new CustomErrorLogger());
        //        }

        //        private void ConfigureCompression(HttpConfiguration config)
        //        {
        //            config.MessageHandlers.Insert(0, new CompressionHandler()); // first runs last
        //        }

        //        private void RemoveUnwantedHeaders(IAppBuilder app)
        //        {
        //            app.Use((context, next) =>
        //            {
        //                context.Response.Headers.Remove("Server");
        //                return next.Invoke();
        //            });
        //        }


        private readonly AppSettings _settings;

        public Startup(IConfiguration configuration)
        {
            _settings = configuration.Get<AppSettings>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            AddDependencies(services);
            AddMvc(services);
            AddCors(services);
            AddAuthorization(services);
            AddAuthentication(services);
            AddSwagger(services);
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.Auth.Secret)),
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

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Poker Bunch Api", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
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
            services.AddSingleton(new UrlProvider(_settings.Urls.Api, _settings.Urls.Site));
            services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();
        }
    }
}
