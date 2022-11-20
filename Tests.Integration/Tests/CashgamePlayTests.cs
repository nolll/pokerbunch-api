using System.Net;
using System.Text.Json;
using Api.Models.CashgameModels;
using Api.Urls.ApiUrls;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.CashgamePlay)]
public class CashgamePlayTests
{
    [Test]
    [Order(1)]
    public async Task AddCashgame()
    {
        var parameters = new AddCashgamePostModel(TestData.BunchLocationId);
        var url = new ApiCashgameAddUrl(TestData.BunchId).Relative;
        var response = await TestSetup.AuthorizedClient(TestData.UserToken).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<CashgameDetailsModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(TestData.CashgameId));
        Assert.That(result.IsRunning, Is.EqualTo(true));
    }

    [Test]
    [Order(2)]
    public async Task Buyin()
    {
        await Buyin(TestData.ManagerToken, TestData.CashgameId, TestData.ManagerPlayerIdInt, 100);
        await Buyin(TestData.UserToken, TestData.CashgameId, TestData.UserPlayerIdInt, 200);
        await Buyin(TestData.ManagerToken, TestData.CashgameId, TestData.PlayerPlayerIdInt, 100);
        await Buyin(TestData.ManagerToken, TestData.CashgameId, TestData.ManagerPlayerIdInt, 100, 50);
    }

    [Test]
    [Order(3)]
    public async Task Report()
    {
        await Report(TestData.ManagerToken, TestData.CashgameId, TestData.ManagerPlayerIdInt, 75);
        await Report(TestData.UserToken, TestData.CashgameId, TestData.UserPlayerIdInt, 265);
        await Report(TestData.ManagerToken, TestData.CashgameId, TestData.PlayerPlayerIdInt, 175);
    }

    [Test]
    [Order(4)]
    public async Task GetCurrentCashgameId()
    {
        var url = new ApiBunchCashgamesCurrentUrl(TestData.BunchId).Relative;
        var content = await TestSetup.AuthorizedClient(TestData.UserToken).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<IEnumerable<ApiCurrentGame>>(content)!.ToList();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result.First().Id, Is.EqualTo(TestData.CashgameId));
    }

    [Test]
    [Order(5)]
    public async Task GetRunningCashgame()
    {
        var url = new ApiCashgameUrl(TestData.CashgameId).Relative;
        var content = await TestSetup.AuthorizedClient(TestData.UserToken).GetStringAsync(url);
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

    [Test]
    [Order(6)]
    public async Task Cashout()
    {
        await Cashout(TestData.UserToken, TestData.CashgameId, TestData.UserPlayerIdInt, 255);
        await Cashout(TestData.ManagerToken, TestData.CashgameId, TestData.ManagerPlayerIdInt, 85);
        await Cashout(TestData.ManagerToken, TestData.CashgameId, TestData.PlayerPlayerIdInt, 310);
    }

    [Test]
    [Order(7)]
    public async Task GetFinishedCashgame()
    {
        var url = new ApiCashgameUrl(TestData.CashgameId).Relative;
        var content = await TestSetup.AuthorizedClient(TestData.UserToken).GetStringAsync(url);
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

    private async Task Buyin(string token, string cashgameId, int playerId, int buyin, int leftInStack = 0)
    {
        var url = new ApiActionAddUrl(cashgameId).Relative;
        var parameters = new AddCashgameActionPostModel("buyin", playerId, buyin, leftInStack);
        var response = await TestSetup.AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task Report(string token, string cashgameId, int playerId, int stack)
    {
        var url = new ApiActionAddUrl(cashgameId).Relative;
        var parameters = new AddCashgameActionPostModel("report", playerId, 0, stack);
        var response = await TestSetup.AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task Cashout(string token, string cashgameId, int playerId, int stack)
    {
        var url = new ApiActionAddUrl(cashgameId).Relative;
        var parameters = new AddCashgameActionPostModel("cashout", playerId, 0, stack);
        var response = await TestSetup.AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}