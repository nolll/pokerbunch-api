using Api.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Bootstrapping
{
    public class Startup
    {
        // Memcache
        // Injection
        // Error handling
        // Https

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
