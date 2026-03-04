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

public class WebApplicationInTest(
    IEmailSender emailSender,
    string connectionString) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.Remove(services.Single(x => x.ServiceType == typeof(IDbContextOptionsConfiguration<PokerBunchDbContext>)));
            services.AddDbContext<PokerBunchDbContext>(options => options.UseNpgsql(connectionString));
            services.ReplaceTransient(emailSender);
            services.ReplaceTransient<IRandomizer>(new FakeRandomizer());
            services.ReplaceTransient<ICache>(new FakeCache());
        });
    }
}
