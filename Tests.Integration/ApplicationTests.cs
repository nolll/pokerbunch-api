using System.Net;
using System.Text.Json;
using Api.Models.BunchModels;
using Api.Models.PlayerModels;
using Api.Models.UserModels;
using Api.Routes;
using Core.UseCases;
using Infrastructure.Sql;
using Tests.Common.FakeServices;

namespace Tests.Integration;

public class ApplicationTests
{
    private WebApplicationFactoryInTest _webApplicationFactory;

    private const string AdminUserName = "admin";
    private const string AdminDisplayName = "Admin";
    private const string AdminEmail = "admin@example.org";
    private const string AdminPassword = "adminpassword";

    private const string ManagerUserName = "manager";
    private const string ManagerDisplayName = "Manager";
    private const string ManagerEmail = "manager@example.org";
    private const string ManagerPassword = "managerpassword";

    private const string UserUserName = "user";
    private const string UserDisplayName = "User";
    private const string UserEmail = "user@example.org";
    private const string UserPassword = "userpassword";

    private const string BunchDisplayName = "Bunch 1";
    private const string BunchSlug = "bunch-1";
    private const string BunchDescription = "Bunch Description 1";
    private const string CurrencySymbol = "$";
    private const string CurrencyLayout = "{SYMBOL}{AMOUNT}";
    private const string TimeZone = "Europe/Stockholm";

    [Test]
    public async Task TestEverything()
    {
        var emailSender = new FakeEmailSender();
        _webApplicationFactory = new WebApplicationFactoryInTest(DatabaseHandler.ConnectionString, emailSender);

        VerifyMasterData();
        await Version();
        await RegisterAdmin();
        var adminValidationCode = emailSender.LastMessage;
        await RegisterManager();
        await RegisterRegularUser();
        var adminToken = await Login(AdminUserName, AdminPassword);
        var managerToken = await Login(ManagerUserName, ManagerPassword);
        var userToken = await Login(UserUserName, UserPassword);
        await CreateBunch(managerToken);
        await AddPlayer(managerToken);
        await JoinBunch(BunchSlug, userToken, adminValidationCode.Body);
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
        await db.ExecuteAsync("UPDATE pb_user SET role = 3 WHERE user_id = 1");
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
        Assert.That(result.DefaultBuyin, Is.EqualTo(200));
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

    private async Task AddPlayer(string token)
    {
        const string playerName = "Player Name";

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
        Assert.That(result.Id, Is.EqualTo(2));
        Assert.That(result.Slug, Is.EqualTo(BunchSlug));
        Assert.That(result.Color, Is.EqualTo("#9e9e9e"));
        Assert.That(result.AvatarUrl, Is.EqualTo(""));
        Assert.That(result.UserId, Is.EqualTo(""));
        Assert.That(result.UserName, Is.EqualTo(""));
    }

    private async Task JoinBunch(string token, string bunchId, string validationCode)
    {
        var url = ApiRoutes.Bunch.Join.Replace("{bunchId}", bunchId);
        var parameters = new JoinBunchPostModel
        {
            Code = validationCode
        };
        var response = await AuthorizedClient(token).PostAsJsonAsync(url, parameters);
    }

    private HttpClient Client => _webApplicationFactory.CreateClient();
    private HttpClient AuthorizedClient(string token)
    {
        var client = _webApplicationFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        return client;
    }
}