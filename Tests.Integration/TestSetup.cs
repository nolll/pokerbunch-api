using Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Tests.Common.FakeServices;
using Xunit;

namespace Tests.Integration;

[CollectionDefinition(nameof(TestSetup))]
public class TestSetup : ICollectionFixture<TestSetup>, IDisposable
{
    private readonly WebApplicationInTest? _webApplicationFactory;
    public readonly FakeEmailSender? EmailSender;

    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:17.6")
        .Build();

    public PokerBunchDbContext? Db { get; }
    public ApiClientForTest ApiClient { get; }
    public LoginHelper LoginHelper { get; }
    
    public TestSetup()
    {
        EmailSender = new FakeEmailSender();
        Task.Run(() => _postgres.StartAsync()).GetAwaiter().GetResult();
        var optionsBuilder = new DbContextOptionsBuilder<PokerBunchDbContext>()
            .UseNpgsql(_postgres.GetConnectionString());
        Db = new PokerBunchDbContext(optionsBuilder.Options);
        Db.Database.EnsureCreated();

        var player = new PbRole { RoleId = 1, RoleName = "Player" };
        var manager = new PbRole { RoleId = 2, RoleName = "Manager" };
        var admin = new PbRole { RoleId = 3, RoleName = "Admin" };
        
        Db.PbRole.AddRange(player, manager, admin);
        Db.SaveChanges();
        
        _webApplicationFactory = new WebApplicationInTest(EmailSender, _postgres.GetConnectionString());
        ApiClient = new ApiClientForTest(new HttpClientForTest(_webApplicationFactory));
        LoginHelper = new LoginHelper(ApiClient);
    }

    public void Dispose()
    {
        if(_webApplicationFactory is not null)
            _webApplicationFactory.Dispose();
        if(Db is not null)
            Db.Dispose();
        Task.Run(() => _postgres.DisposeAsync());
    }
}