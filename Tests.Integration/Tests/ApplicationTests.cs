using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using Api.Models.BunchModels;
using Api.Models.CashgameModels;
using Api.Models.LocationModels;
using Api.Models.PlayerModels;
using Api.Urls.ApiUrls;
using Core;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.Admin)]
public class AdminTests
{
    [Test]
    [Order(1)]
    public async Task ClearCacheAsAdmin()
    {
        var url = new ApiAdminClearCacheUrl().Relative;
        var response = await TestSetup.AuthorizedClient(TestData.AdminToken).PostAsync(url, null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(2)]
    public async Task ClearCacheAsManager()
    {
        var url = new ApiAdminClearCacheUrl().Relative;
        var response = await TestSetup.AuthorizedClient(TestData.ManagerToken).PostAsync(url, null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    [Order(3)]
    public async Task SendTestEmailAsAdmin()
    {
        var url = new ApiAdminSendEmailUrl().Relative;
        var response = await TestSetup.AuthorizedClient(TestData.AdminToken).PostAsync(url, null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(4)]
    public async Task SendTestEmailAsManager()
    {
        var url = new ApiAdminSendEmailUrl().Relative;
        var response = await TestSetup.AuthorizedClient(TestData.ManagerToken).PostAsync(url, null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }
}

[TestFixture]
[NonParallelizable]
[Order(TestOrder.Application)]
public class ApplicationTests
{
    /* Routes left to test */
    //Root
    //Settings
    //Error
    //Swagger
    //Action.Get
    //Action.List
    //Bunch.List
    //Bunch.ListForCurrentUser
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

    [Test]
    public async Task TestApplication()
    {
        await CreateBunch(TestData.ManagerToken);
        await AddPlayerForUser(TestData.ManagerToken);
        var verificationCode = await InviteUserToBunch(TestData.ManagerToken);
        await JoinBunch(TestData.UserToken, verificationCode);
        await AddPlayerWithoutUser(TestData.ManagerToken);
        await GetBunchAsAdmin(TestData.AdminToken);
        await GetBunchAsManager(TestData.ManagerToken);
        await GetBunchAsUser(TestData.UserToken);
        
        await AddLocation(TestData.ManagerToken);
        await ListLocations(TestData.ManagerToken);
        await GetLocation(TestData.ManagerToken);
        
        await AddCashgame(TestData.UserToken, TestData.CashgameId);
        await CashgameBuyin(TestData.ManagerToken, TestData.CashgameId, TestData.ManagerPlayerIdInt, 100);
        await CashgameBuyin(TestData.UserToken, TestData.CashgameId, TestData.UserPlayerIdInt, 200);
        await CashgameBuyin(TestData.ManagerToken, TestData.CashgameId, TestData.PlayerPlayerIdInt, 100);
        await CashgameBuyin(TestData.ManagerToken, TestData.CashgameId, TestData.ManagerPlayerIdInt, 100, 50);
        await CashgameReport(TestData.ManagerToken, TestData.CashgameId, TestData.ManagerPlayerIdInt, 75);
        await CashgameReport(TestData.UserToken, TestData.CashgameId, TestData.UserPlayerIdInt, 265);
        await CashgameReport(TestData.ManagerToken, TestData.CashgameId, TestData.PlayerPlayerIdInt, 175);

        var cashgameId = await GetCurrentCashgameId(TestData.UserToken);
        await GetRunningCashgame(TestData.UserToken, cashgameId);

        await CashgameCashout(TestData.UserToken, TestData.CashgameId, TestData.UserPlayerIdInt, 255);
        await CashgameCashout(TestData.ManagerToken, TestData.CashgameId, TestData.ManagerPlayerIdInt, 85);
        await CashgameCashout(TestData.ManagerToken, TestData.CashgameId, TestData.PlayerPlayerIdInt, 310);
        await GetFinishedCashgame(TestData.UserToken, cashgameId);
    }

    private async Task CreateBunch(string token)
    {
        var parameters = new AddBunchPostModel(TestData.BunchDisplayName, TestData.BunchDescription, TestData.TimeZone, TestData.CurrencySymbol, TestData.CurrencyLayout);
        var response = await TestSetup.AuthorizedClient(token).PostAsJsonAsync(new ApiBunchAddUrl().Relative, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<BunchModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(TestData.BunchDisplayName));
        Assert.That(result.Id, Is.EqualTo(TestData.BunchId));
        Assert.That(result.DefaultBuyin, Is.EqualTo(0));
        Assert.That(result.CurrencySymbol, Is.EqualTo(TestData.CurrencySymbol));
        Assert.That(result.CurrencyLayout, Is.EqualTo(TestData.CurrencyLayout));
        Assert.That(result.CurrencyFormat, Is.EqualTo("${0}"));
        Assert.That(result.Description, Is.EqualTo(TestData.BunchDescription));
        Assert.That(result.HouseRules, Is.EqualTo(""));
        Assert.That(result.Role, Is.EqualTo("manager"));
        Assert.That(result.Timezone, Is.EqualTo(TestData.TimeZone));
        Assert.That(result.ThousandSeparator, Is.EqualTo(" "));
        Assert.That(result.Player.Id, Is.EqualTo("1"));
        Assert.That(result.Player.Name, Is.EqualTo(TestData.ManagerDisplayName));
    }

    private async Task AddPlayerForUser(string token)
    {
        await AddPlayer(token, TestData.UserPlayerName, TestData.UserPlayerIdString);
    }

    private async Task AddPlayerWithoutUser(string token)
    {
        await AddPlayer(token, TestData.PlayerName, TestData.PlayerPlayerIdString);
    }

    private async Task AddPlayer(string token, string playerName, string expectedId)
    {
        var parameters = new PlayerAddPostModel(playerName);
        var url = new ApiPlayerAddUrl(TestData.BunchId).Relative;
        var response = await TestSetup.AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PlayerModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(playerName));
        Assert.That(result.Id, Is.EqualTo(expectedId));
        Assert.That(result.Slug, Is.EqualTo(TestData.BunchId));
        Assert.That(result.Color, Is.EqualTo("#9e9e9e"));
        Assert.That(result.AvatarUrl, Is.EqualTo(""));
        Assert.That(result.UserId, Is.EqualTo(""));
        Assert.That(result.UserName, Is.EqualTo(""));
    }

    private async Task<string> InviteUserToBunch(string token)
    {
        var parameters = new PlayerInvitePostModel(TestData.UserEmail);
        var url = new ApiPlayerInviteUrl(TestData.UserPlayerIdString).Relative;
        await TestSetup.AuthorizedClient(token).PostAsJsonAsync(url, parameters);

        return GetVerificationCode(TestSetup.EmailSender.LastMessage);
    }

    private string GetVerificationCode(IMessage message)
    {
        var regex = new Regex("verification code: (.+)");
        return regex.Match(message.Body).Groups[1].Value.Trim();
    }

    private async Task JoinBunch(string token, string validationCode)
    {
        var url = new ApiBunchJoinUrl(TestData.BunchId).Relative;
        var parameters = new JoinBunchPostModel(validationCode);
        var response = await TestSetup.AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task GetBunchAsAdmin(string token)
    {
        var url = new ApiBunchUrl(TestData.BunchId).Relative;
        var content = await TestSetup.AuthorizedClient(token).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<BunchModel>(content);
        Assert.That(result, Is.Not.Null);
        AssertCommonUserProperties(result);
        Assert.That(result.Role, Is.EqualTo("admin"));
        Assert.That(result.Player, Is.Null);
    }

    private async Task GetBunchAsManager(string token)
    {
        var url = new ApiBunchUrl(TestData.BunchId).Relative;
        var content = await TestSetup.AuthorizedClient(token).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<BunchModel>(content);
        Assert.That(result, Is.Not.Null);
        AssertCommonUserProperties(result);
        Assert.That(result.Role, Is.EqualTo("manager"));
        Assert.That(result.Player.Id, Is.EqualTo(TestData.ManagerPlayerIdString));
        Assert.That(result.Player.Name, Is.EqualTo(TestData.ManagerDisplayName));
    }

    private async Task GetBunchAsUser(string token)
    {
        var url = new ApiBunchUrl(TestData.BunchId).Relative;
        var content = await TestSetup.AuthorizedClient(token).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<BunchModel>(content);
        Assert.That(result, Is.Not.Null);
        AssertCommonUserProperties(result);
        Assert.That(result.Role, Is.EqualTo("player"));
        Assert.That(result.Player.Id, Is.EqualTo(TestData.UserPlayerIdString));
        Assert.That(result.Player.Name, Is.EqualTo(TestData.UserDisplayName));
    }

    private void AssertCommonUserProperties(BunchModel bunch)
    {
        Assert.That(bunch.Name, Is.EqualTo(TestData.BunchDisplayName));
        Assert.That(bunch.Id, Is.EqualTo(TestData.BunchId));
        Assert.That(bunch.DefaultBuyin, Is.EqualTo(0));
        Assert.That(bunch.CurrencySymbol, Is.EqualTo(TestData.CurrencySymbol));
        Assert.That(bunch.CurrencyLayout, Is.EqualTo(TestData.CurrencyLayout));
        Assert.That(bunch.CurrencyFormat, Is.EqualTo("${0}"));
        Assert.That(bunch.Description, Is.EqualTo(TestData.BunchDescription));
        Assert.That(bunch.HouseRules, Is.EqualTo(""));
        Assert.That(bunch.Timezone, Is.EqualTo(TestData.TimeZone));
        Assert.That(bunch.ThousandSeparator, Is.EqualTo(" "));
    }

    private async Task AddLocation(string token)
    {
        var parameters = new LocationAddPostModel(TestData.BunchLocationName);
        var url = new ApiLocationAddUrl(TestData.BunchId).Relative;
        var response = await TestSetup.AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<LocationModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(TestData.BunchLocationId));
    }

    private async Task ListLocations(string token)
    {
        var url = new ApiLocationListUrl(TestData.BunchId).Relative;
        var content = await TestSetup.AuthorizedClient(token).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<List<LocationModel>>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        var location = result[0];
        Assert.That(location.Id, Is.EqualTo(TestData.BunchLocationId));
        Assert.That(location.Name, Is.EqualTo(TestData.BunchLocationName));
        Assert.That(location.Bunch, Is.EqualTo(TestData.BunchId));
    }

    private async Task GetLocation(string token)
    {
        var url = new ApiLocationUrl(TestData.BunchLocationId).Relative;
        var content = await TestSetup.AuthorizedClient(token).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<LocationModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(TestData.BunchLocationId));
        Assert.That(result.Name, Is.EqualTo(TestData.BunchLocationName));
        Assert.That(result.Bunch, Is.EqualTo(TestData.BunchId));
    }

    private async Task AddCashgame(string token, string expectedCashgameId)
    {
        var parameters = new AddCashgamePostModel(TestData.BunchLocationId);
        var url = new ApiCashgameAddUrl(TestData.BunchId).Relative;
        var response = await TestSetup.AuthorizedClient(token).PostAsJsonAsync(url, parameters);
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
        var response = await TestSetup.AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task CashgameReport(string token, string cashgameId, int playerId, int stack)
    {
        var url = new ApiActionAddUrl(cashgameId).Relative;
        var parameters = new AddCashgameActionPostModel("report", playerId, 0, stack);
        var response = await TestSetup.AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task CashgameCashout(string token, string cashgameId, int playerId, int stack)
    {
        var url = new ApiActionAddUrl(cashgameId).Relative;
        var parameters = new AddCashgameActionPostModel("cashout", playerId, 0, stack);
        var response = await TestSetup.AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task<string> GetCurrentCashgameId(string token)
    {
        var url = new ApiBunchCashgamesCurrentUrl(TestData.BunchId).Relative;
        var content = await TestSetup.AuthorizedClient(token).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<IEnumerable<ApiCurrentGame>>(content)!.ToList();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));

        return result.First().Id;
    }

    private async Task GetRunningCashgame(string token, string cashgameId)
    {
        var url = new ApiCashgameUrl(cashgameId).Relative;
        var content = await TestSetup.AuthorizedClient(token).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<CashgameDetailsModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo("1"));
        Assert.That(result.IsRunning, Is.True);
        Assert.That(result.Players.Count, Is.EqualTo(3));

        Assert.That(result.Players[0].Name, Is.EqualTo("Player Name"));
        Assert.That(result.Players[0].Actions[0].Type, Is.EqualTo("buyin"));
        Assert.That(result.Players[0].Actions[0].Added, Is.EqualTo(100));
        Assert.That(result.Players[0].Actions[1].Type, Is.EqualTo("report"));
        Assert.That(result.Players[0].Actions[1].Stack, Is.EqualTo(175));

        Assert.That(result.Players[1].Name, Is.EqualTo("User"));
        Assert.That(result.Players[1].Actions[0].Type, Is.EqualTo("buyin"));
        Assert.That(result.Players[1].Actions[0].Added, Is.EqualTo(200));
        Assert.That(result.Players[1].Actions[1].Type, Is.EqualTo("report"));
        Assert.That(result.Players[1].Actions[1].Stack, Is.EqualTo(265));

        Assert.That(result.Players[2].Name, Is.EqualTo("Manager"));
        Assert.That(result.Players[2].Actions[0].Type, Is.EqualTo("buyin"));
        Assert.That(result.Players[2].Actions[0].Added, Is.EqualTo(100));
        Assert.That(result.Players[2].Actions[1].Type, Is.EqualTo("buyin"));
        Assert.That(result.Players[2].Actions[1].Added, Is.EqualTo(100));
        Assert.That(result.Players[2].Actions[1].Stack, Is.EqualTo(150));
        Assert.That(result.Players[2].Actions[2].Type, Is.EqualTo("report"));
        Assert.That(result.Players[2].Actions[2].Stack, Is.EqualTo(75));
    }

    private async Task GetFinishedCashgame(string token, string cashgameId)
    {
        var url = new ApiCashgameUrl(cashgameId).Relative;
        var content = await TestSetup.AuthorizedClient(token).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<CashgameDetailsModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo("1"));
        Assert.That(result.IsRunning, Is.False);
        Assert.That(result.Players.Count, Is.EqualTo(3));

        Assert.That(result.Players[0].Name, Is.EqualTo("Player Name"));
        Assert.That(result.Players[0].Actions[2].Type, Is.EqualTo("cashout"));
        Assert.That(result.Players[0].Actions[2].Stack, Is.EqualTo(310));

        Assert.That(result.Players[1].Name, Is.EqualTo("User"));
        Assert.That(result.Players[1].Actions[2].Type, Is.EqualTo("cashout"));
        Assert.That(result.Players[1].Actions[2].Stack, Is.EqualTo(255));

        Assert.That(result.Players[2].Name, Is.EqualTo("Manager"));
        Assert.That(result.Players[2].Actions[3].Type, Is.EqualTo("cashout"));
        Assert.That(result.Players[2].Actions[3].Stack, Is.EqualTo(85));
    }
}