using Core.Services;
using Infrastructure.Sql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Tests.Common.FakeServices;

namespace Tests.Integration;

public class WebApplicationFactoryInTest(IEmailSender emailSender, IDb db) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.ReplaceSingleton(db);
            services.ReplaceSingleton(emailSender);
            services.ReplaceSingleton<IRandomizer>(new FakeRandomizer());
            services.ReplaceSingleton<ICache>(new FakeCache());
        });
    }
}