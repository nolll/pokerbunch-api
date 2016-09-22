using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Api;
using Api.Auth;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using Web.Common;
using Web.Common.Urls.ApiUrls;

[assembly: OwinStartup(typeof(Startup))]
namespace Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            ConfigureOAuth(app);
            ConfigRoutes(config);
            ConfigFormatters(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString(new TokenUrl().Relative),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(30),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private static void ConfigRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("default", "{controller}/{id}", new { id = RouteParameter.Optional });
        }

        private static void ConfigFormatters(HttpConfiguration config)
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            config.Formatters.Clear();
            config.Formatters.Add(jsonFormatter);
            config.Formatters.Add(new XmlMediaTypeFormatter());
        }
    }
}