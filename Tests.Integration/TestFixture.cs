using Api.Models.BunchModels;
using Api.Models.PlayerModels;
using Api.Models.UserModels;
using Core.Entities;
using Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Tests.Common;
using Tests.Common.FakeServices;
using Xunit;

namespace Tests.Integration;

[CollectionDefinition(nameof(TestFixture))]
public class TestFixture : ICollectionFixture<TestFixture>, IDisposable
{
    private readonly WebApplicationInTest _webApplicationFactory;
    public readonly FakeEmailSender EmailSender;

    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:17.6")
        .Build();

    public PokerBunchDbContext Db { get; }
    public ApiClientForTest ApiClient { get; }
    public LoginHelper LoginHelper { get; }
    public TestDataFactory DataFactory { get; }
    public TestData Data { get; }
    
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
        Data = new TestData(DataFactory);
        LoginHelper = new LoginHelper(ApiClient, Data);
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
        return new BunchFixture(Db, ApiClient, DataFactory, result.Model!.Id, parameters);
    }
    
    public async Task<UserFixture> CreateUser(
        string? userName = null, 
        string? displayName = null,
        string? email = null,
        string? password = null,
        bool isAdmin = false)
    {
        var parameters = new AddUserPostModel(
            userName ?? DataFactory.String(), 
            displayName ?? DataFactory.String(), 
            email ?? DataFactory.EmailAddress(), 
            password ?? DataFactory.String());
        await ApiClient.User.Add(parameters);
        if (isAdmin)
            await SetAdminRole(parameters.UserName);
        var loginResult = await ApiClient.Auth.Login(new LoginPostModel(parameters.UserName, parameters.Password));
        var accessToken = loginResult.Model!.AccessToken;
        var refreshToken = loginResult.Model!.RefreshToken;
        return new UserFixture(ApiClient, parameters, accessToken, refreshToken);
    }
    
    private async Task SetAdminRole(string? userName)
    {
        var admin = Db.PbUser
            .First(o => o.UserName == userName);

        admin.RoleId = (int)Role.Admin;
        await Db.SaveChangesAsync();
    }
    
    public void Dispose()
    { 
        _webApplicationFactory.Dispose();
        Db.Dispose();
        Task.Run(() => _postgres.DisposeAsync());
    }
}

public class UserFixture(ApiClientForTest apiClient, AddUserPostModel parameters, string token, string refreshToken)
{
    public string UserName { get; } = parameters.UserName;
    public string Password { get; } = parameters.Password;
    public string Email { get; } = parameters.Email;
    public string Token { get; private set; } = token;
    public string RefreshToken { get; } = refreshToken;
    
    public async Task Refresh()
    {
        var result = await apiClient.Auth.Refresh(new(RefreshToken));
        
        if (result.Success)
            Token = result.Model!.AccessToken;
        else
            throw new Exception("Refresh failed");
    }
}

public class BunchFixture(PokerBunchDbContext db, ApiClientForTest apiClient, TestDataFactory dataFactory, string id, AddBunchPostModel parameters)
{
    public string Id { get; } = id;
    public string Name { get; } = parameters.Name;
    public string CurrencySymbol { get; } = parameters.CurrencySymbol;
    public string CurrencyLayout { get; } = parameters.CurrencyLayout;
    public string Description { get; } = parameters.Description;
    public string Timezone { get; } = parameters.Timezone;

    public async Task AddPlayer(UserFixture userToAdd)
    {
        var dbUser = db.PbUser.First(o => o.UserName == userToAdd.UserName);
        var dbBunch = db.PbBunch.First(o => o.Name == Id);
        var player = new PbPlayer
        {
            BunchId = dbBunch.BunchId,
            UserId = dbUser.UserId,
            RoleId = (int)Role.Player,
            Approved = true,
            Color = "#000000"
        };

        db.PbPlayer.Add(player);

        await db.SaveChangesAsync();
        await userToAdd.Refresh();
    }
}