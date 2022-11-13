using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using Api.Models.BunchModels;
using Api.Models.CashgameModels;
using Api.Models.LocationModels;
using Api.Models.PlayerModels;
using Api.Models.UserModels;
using Api.Urls.ApiUrls;
using Core;
using Infrastructure.Sql;
using Tests.Common.FakeServices;

namespace Tests.Integration;

public class ApplicationTests
{
    private WebApplicationFactoryInTest _webApplicationFactory;
    private readonly FakeEmailSender _emailSender = new();

    private const string AdminUserName = "admin";
    private const string AdminDisplayName = "Admin";
    private const string AdminEmail = "admin@example.org";
    private const string AdminPassword = "adminpassword";

    private const string ManagerUserName = "manager";
    private const string ManagerDisplayName = "Manager";
    private const string ManagerEmail = "manager@example.org";
    private const string ManagerPassword = "managerpassword";
    private const int ManagerPlayerIdInt = 1;
    private const string ManagerPlayerIdString = "1";

    private const string UserUserName = "user";
    private const string UserDisplayName = "User";
    private const string UserPlayerName = "User Player Name";
    private const string UserEmail = "user@example.org";
    private const string UserPassword = "userpassword";
    private const int UserPlayerIdInt = 2;
    private const string UserPlayerIdString = "2";

    private const string PlayerName = "Player Name";
    private const int PlayerPlayerIdInt = 3;
    private const string PlayerPlayerIdString = "3";

    private const string BunchDisplayName = "Bunch 1";
    private const string BunchId = "bunch-1";
    private const string BunchDescription = "Bunch Description 1";
    private const int BunchLocationId = 1;
    private const string BunchLocationName = "Bunch Location 1";
    private const string CurrencySymbol = "$";
    private const string CurrencyLayout = "{SYMBOL}{AMOUNT}";
    private const string TimeZone = "Europe/Stockholm";

    private const string CashgameId = "1";

    /* Routes left to test */
    //Root
    //Settings
    //Error
    //Swagger
    //Action.Get
    //Action.List
    //Admin.ClearCache
    //Admin.SendEmail
    //Bunch.List
    //Bunch.ListForCurrentUser
    //Cashgame.Get
    //Cashgame.ListByBunch
    //Cashgame.ListByBunchAndYear
    //Cashgame.ListCurrentByBunch
    //Cashgame.ListByEvent
    //Cashgame.ListByPlayer
    //Cashgame.YearsByBunch
    //Event.Get
    //Event.ListByBunch
    //Player.Get
    //Player.ListByBunch
    //Profile.Get
    //Profile.Password
    //Auth.Token
    //User.List

    [Test]
    public async Task TestApplication()
    {
        _webApplicationFactory = new WebApplicationFactoryInTest(DatabaseHandler.ConnectionString, _emailSender);

        VerifyMasterData();
        
        await Version();

        await RegisterAdmin();
        await RegisterManager();
        await RegisterRegularUser();
        var adminToken = await Login(AdminUserName, AdminPassword);
        var managerToken = await Login(ManagerUserName, ManagerPassword);
        var userToken = await Login(UserUserName, UserPassword);
        
        await CreateBunch(managerToken);
        await AddPlayerForUser(managerToken);
        var verificationCode = await InviteUserToBunch(managerToken);
        await JoinBunch(userToken, verificationCode);
        await AddPlayerWithoutUser(managerToken);
        await GetBunchAsAdmin(adminToken);
        await GetBunchAsManager(managerToken);
        await GetBunchAsUser(userToken);
        
        await AddLocation(managerToken);
        await ListLocations(managerToken);
        await GetLocation(managerToken);
        
        await AddCashgame(userToken, CashgameId);
        await CashgameBuyin(managerToken, CashgameId, ManagerPlayerIdInt, 100);
        await CashgameBuyin(userToken, CashgameId, UserPlayerIdInt, 200);
        await CashgameBuyin(managerToken, CashgameId, PlayerPlayerIdInt, 100);
        await CashgameBuyin(managerToken, CashgameId, ManagerPlayerIdInt, 100, 50);
        await CashgameReport(managerToken, CashgameId, ManagerPlayerIdInt, 75);
        await CashgameReport(userToken, CashgameId, UserPlayerIdInt, 265);
        await CashgameReport(managerToken, CashgameId, PlayerPlayerIdInt, 175);

        await GetCurrentCashgame(userToken);

        await CashgameCashout(userToken, CashgameId, UserPlayerIdInt, 255);
        await CashgameCashout(managerToken, CashgameId, ManagerPlayerIdInt, 85);
        await CashgameCashout(managerToken, CashgameId, PlayerPlayerIdInt, 310);
    }

    private void VerifyMasterData()
    {
        var db = new PostgresStorageProvider(DatabaseHandler.ConnectionString);
        var reader = db.Query("SELECT role_id, role_name FROM pb_role ORDER BY role_id");
        var roles = reader.ReadList(RoleInTest.Create);

        Assert.That(roles[0].Id, Is.EqualTo(1));
        Assert.That(roles[0].Name, Is.EqualTo("Player"));
        Assert.That(roles[1].Id, Is.EqualTo(2));
        Assert.That(roles[1].Name, Is.EqualTo("Manager"));
        Assert.That(roles[2].Id, Is.EqualTo(3));
        Assert.That(roles[2].Name, Is.EqualTo("Admin"));
    }

    private async Task Version()
    {
        var response = await Client.GetAsync(new ApiVersionUrl().Relative);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task RegisterAdmin()
    {
        await RegisterUser(AdminUserName, AdminDisplayName, AdminEmail, AdminPassword);
        var db = new PostgresStorageProvider(DatabaseHandler.ConnectionString);
        await db.ExecuteAsync("UPDATE pb_user SET role_id = 3 WHERE user_id = 1");
    }

    private async Task RegisterManager()
    {
        await RegisterUser(ManagerUserName, ManagerDisplayName, ManagerEmail, ManagerPassword);
    }

    private async Task RegisterRegularUser()
    {
        await RegisterUser(UserUserName, UserDisplayName, UserEmail, UserPassword);
    }

    private async Task RegisterUser(string userName, string displayName, string email, string password)
    {
        var parameters = new AddUserPostModel(userName, displayName, email, password);
        var response = await Client.PostAsJsonAsync(new ApiUserAddUrl().Relative, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    private async Task<string> Login(string userName, string password)
    {
        var parameters = new LoginPostModel(userName, password);
        var response = await Client.PostAsJsonAsync(new ApiLoginUrl().Relative, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var token = await response.Content.ReadAsStringAsync();
        Assert.That(token, Is.Not.Empty);

        return token;
    }
    
    private async Task<UserModel> GetUser(string token, string userName)
    {
        var url = new ApiUserUrl(userName).Relative;
        var response = await AuthorizedClient(token).GetAsync(url);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<UserModel>(content);

        return result;
    }

    private async Task CreateBunch(string token)
    {
        var parameters = new AddBunchPostModel(BunchDisplayName, BunchDescription, TimeZone, CurrencySymbol, CurrencyLayout);
        var response = await AuthorizedClient(token).PostAsJsonAsync(new ApiBunchAddUrl().Relative, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<BunchModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(BunchDisplayName));
        Assert.That(result.Id, Is.EqualTo(BunchId));
        Assert.That(result.DefaultBuyin, Is.EqualTo(0));
        Assert.That(result.CurrencySymbol, Is.EqualTo(CurrencySymbol));
        Assert.That(result.CurrencyLayout, Is.EqualTo(CurrencyLayout));
        Assert.That(result.CurrencyFormat, Is.EqualTo("${0}"));
        Assert.That(result.Description, Is.EqualTo(BunchDescription));
        Assert.That(result.HouseRules, Is.EqualTo(""));
        Assert.That(result.Role, Is.EqualTo("manager"));
        Assert.That(result.Timezone, Is.EqualTo(TimeZone));
        Assert.That(result.ThousandSeparator, Is.EqualTo(" "));
        Assert.That(result.Player.Id, Is.EqualTo("1"));
        Assert.That(result.Player.Name, Is.EqualTo(ManagerDisplayName));
    }

    private async Task AddPlayerForUser(string token)
    {
        await AddPlayer(token, UserPlayerName, UserPlayerIdString);
    }

    private async Task AddPlayerWithoutUser(string token)
    {
        await AddPlayer(token, PlayerName, PlayerPlayerIdString);
    }

    private async Task AddPlayer(string token, string playerName, string expectedId)
    {
        var parameters = new PlayerAddPostModel(playerName);
        var url = new ApiPlayerAddUrl(BunchId).Relative;
        var response = await AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PlayerModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(playerName));
        Assert.That(result.Id, Is.EqualTo(expectedId));
        Assert.That(result.Slug, Is.EqualTo(BunchId));
        Assert.That(result.Color, Is.EqualTo("#9e9e9e"));
        Assert.That(result.AvatarUrl, Is.EqualTo(""));
        Assert.That(result.UserId, Is.EqualTo(""));
        Assert.That(result.UserName, Is.EqualTo(""));
    }

    private async Task<string> InviteUserToBunch(string token)
    {
        var parameters = new PlayerInvitePostModel(UserEmail);
        var url = new ApiPlayerInviteUrl(UserPlayerIdString).Relative;
        await AuthorizedClient(token).PostAsJsonAsync(url, parameters);

        return GetVerificationCode(_emailSender.LastMessage);
    }

    private string GetVerificationCode(IMessage message)
    {
        var regex = new Regex("verification code: (.+)");
        return regex.Match(message.Body).Groups[1].Value.Trim();
    }

    private async Task JoinBunch(string token, string validationCode)
    {
        var url = new ApiBunchJoinUrl(BunchId).Relative;
        var parameters = new JoinBunchPostModel(validationCode);
        var response = await AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task GetBunchAsAdmin(string token)
    {
        var url = new ApiBunchUrl(BunchId).Relative;
        var content = await AuthorizedClient(token).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<BunchModel>(content);
        Assert.That(result, Is.Not.Null);
        AssertCommonUserProperties(result);
        Assert.That(result.Role, Is.EqualTo("admin"));
        Assert.That(result.Player, Is.Null);
    }

    private async Task GetBunchAsManager(string token)
    {
        var url = new ApiBunchUrl(BunchId).Relative;
        var content = await AuthorizedClient(token).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<BunchModel>(content);
        Assert.That(result, Is.Not.Null);
        AssertCommonUserProperties(result);
        Assert.That(result.Role, Is.EqualTo("manager"));
        Assert.That(result.Player.Id, Is.EqualTo(ManagerPlayerIdString));
        Assert.That(result.Player.Name, Is.EqualTo(ManagerDisplayName));
    }

    private async Task GetBunchAsUser(string token)
    {
        var url = new ApiBunchUrl(BunchId).Relative;
        var content = await AuthorizedClient(token).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<BunchModel>(content);
        Assert.That(result, Is.Not.Null);
        AssertCommonUserProperties(result);
        Assert.That(result.Role, Is.EqualTo("player"));
        Assert.That(result.Player.Id, Is.EqualTo(UserPlayerIdString));
        Assert.That(result.Player.Name, Is.EqualTo(UserDisplayName));
    }

    private void AssertCommonUserProperties(BunchModel bunch)
    {
        Assert.That(bunch.Name, Is.EqualTo(BunchDisplayName));
        Assert.That(bunch.Id, Is.EqualTo(BunchId));
        Assert.That(bunch.DefaultBuyin, Is.EqualTo(0));
        Assert.That(bunch.CurrencySymbol, Is.EqualTo(CurrencySymbol));
        Assert.That(bunch.CurrencyLayout, Is.EqualTo(CurrencyLayout));
        Assert.That(bunch.CurrencyFormat, Is.EqualTo("${0}"));
        Assert.That(bunch.Description, Is.EqualTo(BunchDescription));
        Assert.That(bunch.HouseRules, Is.EqualTo(""));
        Assert.That(bunch.Timezone, Is.EqualTo(TimeZone));
        Assert.That(bunch.ThousandSeparator, Is.EqualTo(" "));
    }

    private async Task AddLocation(string token)
    {
        var parameters = new LocationAddPostModel(BunchLocationName);
        var url = new ApiLocationAddUrl(BunchId).Relative;
        var response = await AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<LocationModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(BunchLocationId));
    }

    private async Task ListLocations(string token)
    {
        var url = new ApiLocationListUrl(BunchId).Relative;
        var content = await AuthorizedClient(token).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<List<LocationModel>>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        var location = result[0];
        Assert.That(location.Id, Is.EqualTo(BunchLocationId));
        Assert.That(location.Name, Is.EqualTo(BunchLocationName));
        Assert.That(location.Bunch, Is.EqualTo(BunchId));
    }

    private async Task GetLocation(string token)
    {
        var url = new ApiLocationUrl(BunchLocationId).Relative;
        var content = await AuthorizedClient(token).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<LocationModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(BunchLocationId));
        Assert.That(result.Name, Is.EqualTo(BunchLocationName));
        Assert.That(result.Bunch, Is.EqualTo(BunchId));
    }

    private async Task AddCashgame(string token, string expectedCashgameId)
    {
        var parameters = new AddCashgamePostModel(BunchLocationId);
        var url = new ApiCashgameAddUrl(BunchId).Relative;
        var response = await AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<CashgameDetailsModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(expectedCashgameId));
        Assert.That(result.IsRunning, Is.EqualTo(true));
    }

    private async Task CashgameBuyin(string token, string cashgameId, int playerId, int buyin, int leftInStack = 0)
    {
        var url = new ApiActionAddUrl(cashgameId).Relative;
        var parameters = new AddCashgameActionPostModel("buyin", playerId, buyin, leftInStack);
        var response = await AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task CashgameReport(string token, string cashgameId, int playerId, int stack)
    {
        var url = new ApiActionAddUrl(cashgameId).Relative;
        var parameters = new AddCashgameActionPostModel("report", playerId, 0, stack);
        var response = await AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task CashgameCashout(string token, string cashgameId, int playerId, int stack)
    {
        var url = new ApiActionAddUrl(cashgameId).Relative;
        var parameters = new AddCashgameActionPostModel("cashout", playerId, 0, stack);
        var response = await AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task GetCurrentCashgame(string token)
    {
        var url = new ApiBunchCashgamesCurrentUrl(BunchId).Relative;
        var content = await AuthorizedClient(token).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<IEnumerable<ApiCurrentGame>>(content).ToList();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
    }

    private HttpClient Client => _webApplicationFactory.CreateClient();
    private HttpClient AuthorizedClient(string token)
    {
        var client = _webApplicationFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        return client;
    }
}