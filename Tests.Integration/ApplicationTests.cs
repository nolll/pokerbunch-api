using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using Api.Models.BunchModels;
using Api.Models.PlayerModels;
using Api.Models.UserModels;
using Api.Routes;
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
    private const string ManagerPlayerId = "1";

    private const string UserUserName = "user";
    private const string UserDisplayName = "User";
    private const string UserPlayerName = "User Player Name";
    private const string UserEmail = "user@example.org";
    private const string UserPassword = "userpassword";
    private const string UserPlayerId = "2";

    private const string PlayerName = "Player Name";
    private const string PlayerPlayerId = "3";

    private const string BunchDisplayName = "Bunch 1";
    private const string BunchSlug = "bunch-1";
    private const string BunchDescription = "Bunch Description 1";
    private const string CurrencySymbol = "$";
    private const string CurrencyLayout = "{SYMBOL}{AMOUNT}";
    private const string TimeZone = "Europe/Stockholm";

    [Test]
    public async Task TestEverything()
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
        await JoinBunch(userToken, BunchSlug, verificationCode);
        await AddPlayerWithoutUser(managerToken);
        await GetBunchAsAdmin(adminToken, BunchSlug);
        await GetBunchAsManager(managerToken, BunchSlug);
        await GetBunchAsUser(userToken, BunchSlug);
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
        var response = await Client.GetAsync(ApiRoutes.Version);
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
        var parameters = new AddUserPostModel
        {
            UserName = userName,
            DisplayName = displayName,
            Email = email,
            Password = password
        };

        var response = await Client.PostAsJsonAsync(ApiRoutes.User.List, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    private async Task<string> Login(string userName, string password)
    {
        var parameters = new LoginPostModel
        {
            UserName = userName,
            Password = password
        };

        var response = await Client.PostAsJsonAsync(ApiRoutes.Auth.Login, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var token = await response.Content.ReadAsStringAsync();
        Assert.That(token, Is.Not.Empty);

        return token;
    }
    
    private async Task<UserModel> GetUser(string token, string userName)
    {
        var url = ApiRoutes.User.Get.Replace("{userName}", userName);
        var response = await AuthorizedClient(token).GetAsync(url);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<UserModel>(content);

        return result;
    }

    private async Task CreateBunch(string token)
    {
        var parameters = new AddBunchPostModel
        {
            Name = BunchDisplayName,
            Description = BunchDescription,
            Timezone = TimeZone,
            CurrencySymbol = CurrencySymbol,
            CurrencyLayout = CurrencyLayout
        };

        var response = await AuthorizedClient(token).PostAsJsonAsync(ApiRoutes.Bunch.List, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<BunchModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(BunchDisplayName));
        Assert.That(result.Id, Is.EqualTo(BunchSlug));
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
        await AddPlayer(token, UserPlayerName, UserPlayerId);
    }

    private async Task AddPlayerWithoutUser(string token)
    {
        await AddPlayer(token, PlayerName, PlayerPlayerId);
    }

    private async Task AddPlayer(string token, string playerName, string expectedId)
    {
        var parameters = new PlayerAddPostModel
        {
            Name = playerName
        };

        var url = ApiRoutes.Player.ListByBunch.Replace("{bunchId}", BunchSlug);
        var response = await AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PlayerModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(playerName));
        Assert.That(result.Id, Is.EqualTo(expectedId));
        Assert.That(result.Slug, Is.EqualTo(BunchSlug));
        Assert.That(result.Color, Is.EqualTo("#9e9e9e"));
        Assert.That(result.AvatarUrl, Is.EqualTo(""));
        Assert.That(result.UserId, Is.EqualTo(""));
        Assert.That(result.UserName, Is.EqualTo(""));
    }

    private async Task<string> InviteUserToBunch(string token)
    {
        var parameters = new PlayerInvitePostModel
        {
            Email = UserEmail
        };

        var url = ApiRoutes.Player.Invite.Replace("{playerId}", UserPlayerId);
        await AuthorizedClient(token).PostAsJsonAsync(url, parameters);

        return GetVerificationCode(_emailSender.LastMessage);
    }

    private string GetVerificationCode(IMessage message)
    {
        var regex = new Regex("verification code: (.+)");
        return regex.Match(message.Body).Groups[1].Value.Trim();
    }

    private async Task JoinBunch(string token, string bunchId, string validationCode)
    {
        var url = ApiRoutes.Bunch.Join.Replace("{bunchId}", bunchId);
        var parameters = new JoinBunchPostModel
        {
            Code = validationCode
        };
        var response = await AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task GetBunchAsAdmin(string token, string bunchId)
    {
        var url = ApiRoutes.Bunch.Get.Replace("{bunchId}", bunchId);
        var content = await AuthorizedClient(token).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<BunchModel>(content);
        Assert.That(result, Is.Not.Null);
        AssertCommonUserProperties(result);
        Assert.That(result.Role, Is.EqualTo("admin"));
        Assert.That(result.Player, Is.Null);
    }

    private async Task GetBunchAsManager(string token, string bunchId)
    {
        var url = ApiRoutes.Bunch.Get.Replace("{bunchId}", bunchId);
        var content = await AuthorizedClient(token).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<BunchModel>(content);
        Assert.That(result, Is.Not.Null);
        AssertCommonUserProperties(result);
        Assert.That(result.Role, Is.EqualTo("manager"));
        Assert.That(result.Player.Id, Is.EqualTo(ManagerPlayerId));
        Assert.That(result.Player.Name, Is.EqualTo(ManagerDisplayName));
    }

    private async Task GetBunchAsUser(string token, string bunchId)
    {
        var url = ApiRoutes.Bunch.Get.Replace("{bunchId}", bunchId);
        var content = await AuthorizedClient(token).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<BunchModel>(content);
        Assert.That(result, Is.Not.Null);
        AssertCommonUserProperties(result);
        Assert.That(result.Role, Is.EqualTo("player"));
        Assert.That(result.Player.Id, Is.EqualTo(UserPlayerId));
        Assert.That(result.Player.Name, Is.EqualTo(UserDisplayName));
    }

    private void AssertCommonUserProperties(BunchModel bunch)
    {
        Assert.That(bunch.Name, Is.EqualTo(BunchDisplayName));
        Assert.That(bunch.Id, Is.EqualTo(BunchSlug));
        Assert.That(bunch.DefaultBuyin, Is.EqualTo(0));
        Assert.That(bunch.CurrencySymbol, Is.EqualTo(CurrencySymbol));
        Assert.That(bunch.CurrencyLayout, Is.EqualTo(CurrencyLayout));
        Assert.That(bunch.CurrencyFormat, Is.EqualTo("${0}"));
        Assert.That(bunch.Description, Is.EqualTo(BunchDescription));
        Assert.That(bunch.HouseRules, Is.EqualTo(""));
        Assert.That(bunch.Timezone, Is.EqualTo(TimeZone));
        Assert.That(bunch.ThousandSeparator, Is.EqualTo(" "));
    }

    private HttpClient Client => _webApplicationFactory.CreateClient();
    private HttpClient AuthorizedClient(string token)
    {
        var client = _webApplicationFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        return client;
    }
}