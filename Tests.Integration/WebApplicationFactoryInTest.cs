using Api;
using Core.Services;
using Infrastructure.Sql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Tests.Common.FakeServices;

namespace Tests.Integration;

public class WebApplicationFactoryInTest : WebApplicationFactory<Program>
{
    private readonly string _connectionString;
    private readonly IEmailSender _emailSender;

    public WebApplicationFactoryInTest(string connectionString, IEmailSender emailSender)
    {
        _connectionString = connectionString;
        _emailSender = emailSender;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.ReplaceSingleton(new PostgresDb(_connectionString));
            services.ReplaceSingleton(_emailSender);
            services.ReplaceSingleton<IRandomizer>(new FakeRandomizer());
            services.ReplaceSingleton<ICacheProvider>(new FakeCacheProvider());
       });
    }
}