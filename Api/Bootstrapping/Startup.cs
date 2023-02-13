using Api.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Bootstrapping;

public class Startup
{
    private readonly AppSettings _settings;
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _settings = configuration.Get<AppSettings>()!;
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        new ServiceConfig(_settings, services, _configuration).Configure();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        new AppConfig(_settings, app, env).Configure();
    }
}