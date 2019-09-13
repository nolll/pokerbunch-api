using Api.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api.Bootstrapping
{
    public class Startup
    {
        private readonly ILogger _logger;
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
            new ServiceConfig(_settings, services).Configure();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            new AppConfig(app, env).Configure();
        }
    }
}
