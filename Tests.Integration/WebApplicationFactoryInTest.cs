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

    public WebApplicationFactoryInTest(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.ReplaceSingleton(new PostgresStorageProvider(_connectionString));
            services.ReplaceSingleton<IEmailSender>(new FakeEmailSender());
            services.ReplaceSingleton<IRandomizer>(new FakeRandomizer());
            services.ReplaceSingleton<ICacheProvider>(new FakeCacheProvider());
       });
    }
}