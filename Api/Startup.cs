﻿using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using Api;
using Api.Auth;
using Api.Extensions;
using Api.Extensions.Compression;
using JetBrains.Annotations;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using PokerBunch.Common.Urls.ApiUrls;
using Swashbuckle.Application;

[assembly: OwinStartup(typeof(Startup))]
namespace Api
{
    public class Startup
    {
        [UsedImplicitly]
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            ConfigureOAuth(app);
            ConfigRoutes(config);
            ConfigFormatters(config);
            ConfigureErrorHandler(config);
            ConfigureErrorLogger(config);
            ConfigureCompression(config);
            ConfigureSwagger(config);
            RemoveUnwantedHeaders(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString(new ApiTokenUrl().Relative),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(365),
                Provider = new SimpleAuthorizationServerProvider()
            };

            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private static void ConfigRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            // The default routing is needed for 404 handling to kick in
            config.Routes.MapHttpRoute("default", "{controller}/{id}", new { id = RouteParameter.Optional });
        }

        private static void ConfigFormatters(HttpConfiguration config)
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));

            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));

            config.Formatters.Clear();
            config.Formatters.Add(jsonFormatter);
        }

        private void ConfigureErrorHandler(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());
            config.Services.Replace(typeof(IHttpControllerSelector), new HttpNotFoundAwareDefaultHttpControllerSelector(config));
            config.Services.Replace(typeof(IHttpActionSelector), new HttpNotFoundAwareControllerActionSelector());
        }

        private void ConfigureErrorLogger(HttpConfiguration config)
        {
            config.Services.Add(typeof(IExceptionLogger), new CustomErrorLogger());
        }

        private void ConfigureCompression(HttpConfiguration config)
        {
            config.MessageHandlers.Insert(0, new CompressionHandler()); // first runs last
        }

        private void ConfigureSwagger(HttpConfiguration config)
        {
            config
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "A title for your API");
                    c.IncludeXmlComments(GetXmlCommentsPathForControllers());
                })
                .EnableSwaggerUi();
        }

        private void RemoveUnwantedHeaders(IAppBuilder app)
        {
            app.Use((context, next) =>
            {
                context.Response.Headers.Remove("Server");
                return next.Invoke();
            });
        }
    }
}