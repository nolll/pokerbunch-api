using Api.Models.BunchModels;
using Api.Models.UserModels;
using Core.Entities;
using Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Tests.Common;
using Tests.Common.FakeServices;
using Xunit;

namespace Tests.Integration.Fixtures;

[CollectionDefinition(nameof(TestFixture))]
public class TestFixture : ICollectionFixture<TestFixture>, IDisposable
{
    private readonly WebApplicationInTest _webApplicationFactory;
    public readonly FakeEmailSender EmailSender;

    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:17.6")
        .Build();

    public PokerBunchDbContext Db { get; }
    public ApiClientForTest ApiClient { get; }
    public TestDataFactory DataFactory { get; }
    
    public TestFixture()
    {
        EmailSender = new FakeEmailSender();
        Task.Run(() => _postgres.StartAsync()).GetAwaiter().GetResult();
        var optionsBuilder = new DbContextOptionsBuilder<PokerBunchDbContext>()
            .UseNpgsql(_postgres.GetConnectionString());
        Db = new PokerBunchDbContext(optionsBuilder.Options);
        Db.Database.EnsureCreated();

        Db.PbRole.AddRange(
            new PbRole { RoleId = 1, RoleName = nameof(Role.Player) },
            new PbRole { RoleId = 2, RoleName = nameof(Role.Manager) },
            new PbRole { RoleId = 3, RoleName = nameof(Role.Admin) }
        );
        Db.SaveChanges();
        
        _webApplicationFactory = new WebApplicationInTest(EmailSender, _postgres.GetConnectionString());
        ApiClient = new ApiClientForTest(new HttpClientForTest(_webApplicationFactory));
        DataFactory = new TestDataFactory();
    }

    public async Task<BunchFixture> CreateBunch(
        UserFixture user,
        string? displayName = null,
        string? description = null,
        string? timeZone = null,
        string? currencySymbol = null,
        string? currencyLayout = null)
    {
        var parameters = new AddBunchPostModel(
            displayName ?? DataFactory.String(),
            description ?? DataFactory.String(), 
            timeZone ?? "Europe/Stockholm", 
            currencySymbol ?? "$", 
            currencyLayout ?? "{SYMBOL}{AMOUNT}");

        var result = await ApiClient.Bunch.Add(user.Token, parameters);
        await user.Refresh();
        return new BunchFixture(Db, ApiClient, DataFactory, user, result.Model!.Id, parameters);
    }
    
    public async Task<UserFixture> CreateUser(
        string? userName = null, 
        string? displayName = null,
        string? email = null,
        string? password = null)
    {
        var parameters = new AddUserPostModel(
            userName ?? DataFactory.String(), 
            displayName ?? DataFactory.String(), 
            email ?? DataFactory.EmailAddress(), 
            password ?? DataFactory.String());
        await ApiClient.User.Add(parameters);
        var loginResult = await ApiClient.Auth.Login(new LoginPostModel(parameters.UserName, parameters.Password));
        var accessToken = loginResult.Model!.AccessToken;
        var refreshToken = loginResult.Model!.RefreshToken;
        return new UserFixture(Db, ApiClient, parameters, accessToken, refreshToken);
    }
    
    public void Dispose()
    { 
        _webApplicationFactory.Dispose();
        Db.Dispose();
        Task.Run(() => _postgres.DisposeAsync());
    }
}