using Api;
using Core.Services;
using Infrastructure.Sql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Tests.Common.FakeServices;

namespace Tests.Integration;

public class WebApplicationFactoryInTest : WebApplicationFactory<Program>
{
    private readonly IEmailSender _emailSender;
    private readonly IDb _db;

    public WebApplicationFactoryInTest(IEmailSender emailSender, IDb db)
    {
        _emailSender = emailSender;
        _db = db;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.ReplaceSingleton(_db);
            services.ReplaceSingleton(_emailSender);
            services.ReplaceSingleton<IRandomizer>(new FakeRandomizer());
            services.ReplaceSingleton<ICache>(new FakeCache());
        });
    }
}