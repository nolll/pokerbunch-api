using Api.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Bootstrapping
{
    public class Startup
    {
        //        private void ConfigureErrorHandler(HttpConfiguration config)
        //        {
        //            config.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());
        //            config.Services.Replace(typeof(IHttpControllerSelector), new HttpNotFoundAwareDefaultHttpControllerSelector(config));
        //            config.Services.Replace(typeof(IHttpActionSelector), new HttpNotFoundAwareControllerActionSelector());
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
