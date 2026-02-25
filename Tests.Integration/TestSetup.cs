using Core;
using Infrastructure.Sql.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Tests.Common.FakeServices;

namespace Tests.Integration;

[SetUpFixture]
public class TestSetup
{
    private static WebApplicationFactoryInTest? _webApplicationFactory;
    public static FakeEmailSender? EmailSender;
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:17.6")
        .Build();

    public static PokerBunchDbContext? Db { get; private set; }
    
    [OneTimeSetUp]
    public async Task SetUp()
    {
        EmailSender = new FakeEmailSender();
        await _postgres.StartAsync();
        var optionsBuilder = new DbContextOptionsBuilder<PokerBunchDbContext>()
            .UseNpgsql(_postgres.GetConnectionString());
        Db = new PokerBunchDbContext(optionsBuilder.Options);
        await Db.Database.EnsureCreatedAsync();

        var player = new PbRole { RoleId = 1, RoleName = "Player" };
        var manager = new PbRole { RoleId = 2, RoleName = "Manager" };
        var admin = new PbRole { RoleId = 3, RoleName = "Admin" };
        
        Db.PbRole.AddRange(player, manager, admin);
        await Db.SaveChangesAsync();
        
        _webApplicationFactory = new WebApplicationFactoryInTest(EmailSender, _postgres.GetConnectionString());
    }
    
    public static HttpClient GetClient(string? token = null, bool followRedirect = true)
    {
        if (_webApplicationFactory == null)
            throw new PokerBunchException("WebApplicationFactory was not initialized.");

        var options = new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = !followRedirect
        };
        
        var client = _webApplicationFactory.CreateClient(options);

        if(token != null)
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        return client;
    }
    
    [OneTimeTearDown]
    public async Task TearDown()
    {
        if(_webApplicationFactory is not null)
            await _webApplicationFactory.DisposeAsync();
        if(Db is not null)
            await Db.DisposeAsync();
        await _postgres.DisposeAsync();
    }
}