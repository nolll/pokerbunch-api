using Core.Services;
using Infrastructure.Sql.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Tests.Common.FakeServices;

namespace Tests.Integration;

public class WebApplicationFactoryInTest(
    IEmailSender emailSender,
    string connectionString) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.Remove(services.Single(x => x.ServiceType == typeof(IDbContextOptionsConfiguration<PokerBunchDbContext>)));
            services.AddDbContext<PokerBunchDbContext>(options => options.UseNpgsql(connectionString));
            services.ReplaceSingleton(emailSender);
            services.ReplaceSingleton<IRandomizer>(new FakeRandomizer());
            services.ReplaceSingleton<ICache>(new FakeCache());
        });
    }
}
