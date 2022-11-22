using System.Net;
using Api.Models.CashgameModels;

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
        var result = await TestClient.Cashgame.Add(TestData.UserToken, TestData.BunchId, parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model.Id, Is.EqualTo(TestData.CashgameId));
        Assert.That(result.Model.IsRunning, Is.EqualTo(true));
    }

    [Test]
    [Order(2)]
    public async Task Buyin()
    {
        await Buyin(TestData.ManagerToken, TestData.CashgameId, TestData.ManagerPlayerId, 100);
        await Buyin(TestData.UserToken, TestData.CashgameId, TestData.UserPlayerId, 200);
        await Buyin(TestData.ManagerToken, TestData.CashgameId, TestData.PlayerPlayerId, 100);
        await Buyin(TestData.ManagerToken, TestData.CashgameId, TestData.ManagerPlayerId, 100, 50);
    }

    [Test]
    [Order(3)]
    public async Task Report()
    {
        await Report(TestData.ManagerToken, TestData.CashgameId, TestData.ManagerPlayerId, 75);
        await Report(TestData.UserToken, TestData.CashgameId, TestData.UserPlayerId, 265);
        await Report(TestData.ManagerToken, TestData.CashgameId, TestData.PlayerPlayerId, 175);
    }

    [Test]
    [Order(4)]
    public async Task AddUpdateAndDeleteAction()
    {
        const string actionId = "8";
        await Report(TestData.ManagerToken, TestData.CashgameId, TestData.ManagerPlayerId, 5000);
        await Update(TestData.ManagerToken, TestData.CashgameId, actionId, 5001);
        await Delete(TestData.ManagerToken, TestData.CashgameId, actionId);
    }

    [Test]
    [Order(5)]
    public async Task GetCurrentCashgameId()
    {
        var result1 = await TestClient.Cashgame.Current(TestData.UserToken, TestData.BunchId);
        Assert.That(result1.Model, Is.Not.Null);
        Assert.That(result1.Model.Count, Is.EqualTo(1));
        Assert.That(result1.Model.First().Id, Is.EqualTo(TestData.CashgameId));
    }

    [Test]
    [Order(6)]
    public async Task GetRunningCashgame()
    {
        var result1 = await TestClient.Cashgame.Get(TestData.UserToken, TestData.CashgameId);
        Assert.That(result1.Model, Is.Not.Null);
        Assert.That(result1.Model.Id, Is.EqualTo("1"));
        Assert.That(result1.Model.IsRunning, Is.True);
        Assert.That(result1.Model.Players.Count, Is.EqualTo(3));

        Assert.That(result1.Model.Players[0].Name, Is.EqualTo("Player Name"));
        Assert.That(result1.Model.Players[0].Actions[0].Type, Is.EqualTo("buyin"));
        Assert.That(result1.Model.Players[0].Actions[0].Added, Is.EqualTo(100));
        Assert.That(result1.Model.Players[0].Actions[1].Type, Is.EqualTo("report"));
        Assert.That(result1.Model.Players[0].Actions[1].Stack, Is.EqualTo(175));

        Assert.That(result1.Model.Players[1].Name, Is.EqualTo("User"));
        Assert.That(result1.Model.Players[1].Actions[0].Type, Is.EqualTo("buyin"));
        Assert.That(result1.Model.Players[1].Actions[0].Added, Is.EqualTo(200));
        Assert.That(result1.Model.Players[1].Actions[1].Type, Is.EqualTo("report"));
        Assert.That(result1.Model.Players[1].Actions[1].Stack, Is.EqualTo(265));

        Assert.That(result1.Model.Players[2].Name, Is.EqualTo("Manager"));
        Assert.That(result1.Model.Players[2].Actions[0].Type, Is.EqualTo("buyin"));
        Assert.That(result1.Model.Players[2].Actions[0].Added, Is.EqualTo(100));
        Assert.That(result1.Model.Players[2].Actions[1].Type, Is.EqualTo("buyin"));
        Assert.That(result1.Model.Players[2].Actions[1].Added, Is.EqualTo(100));
        Assert.That(result1.Model.Players[2].Actions[1].Stack, Is.EqualTo(150));
        Assert.That(result1.Model.Players[2].Actions[2].Type, Is.EqualTo("report"));
        Assert.That(result1.Model.Players[2].Actions[2].Stack, Is.EqualTo(75));
    }

    [Test]
    [Order(7)]
    public async Task Cashout()
    {
        await Cashout(TestData.UserToken, TestData.CashgameId, TestData.UserPlayerId, 255);
        await Cashout(TestData.ManagerToken, TestData.CashgameId, TestData.ManagerPlayerId, 85);
        await Cashout(TestData.ManagerToken, TestData.CashgameId, TestData.PlayerPlayerId, 310);
    }

    [Test]
    [Order(8)]
    public async Task GetFinishedCashgame()
    {
        var result = await TestClient.Cashgame.Get(TestData.UserToken, TestData.CashgameId);
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model.Id, Is.EqualTo("1"));
        Assert.That(result.Model.IsRunning, Is.False);
        Assert.That(result.Model.Players.Count, Is.EqualTo(3));

        Assert.That(result.Model.Players[0].Name, Is.EqualTo("Player Name"));
        Assert.That(result.Model.Players[0].Actions[2].Type, Is.EqualTo("cashout"));
        Assert.That(result.Model.Players[0].Actions[2].Stack, Is.EqualTo(310));

        Assert.That(result.Model.Players[1].Name, Is.EqualTo("User"));
        Assert.That(result.Model.Players[1].Actions[2].Type, Is.EqualTo("cashout"));
        Assert.That(result.Model.Players[1].Actions[2].Stack, Is.EqualTo(255));

        Assert.That(result.Model.Players[2].Name, Is.EqualTo("Manager"));
        Assert.That(result.Model.Players[2].Actions[3].Type, Is.EqualTo("cashout"));
        Assert.That(result.Model.Players[2].Actions[3].Stack, Is.EqualTo(85));
    }

    private async Task Buyin(string token, string cashgameId, string playerId, int buyin, int leftInStack = 0)
    {
        var parameters = new AddCashgameActionPostModel("buyin", playerId, buyin, leftInStack);
        var result = await TestClient.Action.Add(token, cashgameId, parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task Report(string token, string cashgameId, string playerId, int stack)
    {
        var parameters = new AddCashgameActionPostModel("report", playerId, 0, stack);
        var result = await TestClient.Action.Add(token, cashgameId, parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task Cashout(string token, string cashgameId, string playerId, int stack)
    {
        var parameters = new AddCashgameActionPostModel("cashout", playerId, 0, stack);
        var result = await TestClient.Action.Add(token, cashgameId, parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task Update(string token, string cashgameId, string actionId, int stack)
    {
        var parameters = new UpdateActionPostModel(DateTimeOffset.Now, stack, null);
        var result = await TestClient.Action.Update(token, cashgameId, actionId, parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task Delete(string token, string cashgameId, string actionId)
    {
        var result = await TestClient.Action.Delete(token, cashgameId, actionId);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}