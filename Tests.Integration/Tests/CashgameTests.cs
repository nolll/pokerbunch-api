using System.Net;
using Api.Models.CashgameModels;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.Cashgame)]
public class CashgameTests
{
    [Test]
    [Order(1)]
    public async Task AddCashgame()
    {
        var parameters = new AddCashgamePostModel(TestData.BunchLocationId);
        var result = await TestClient.Cashgame.Add(TestData.UserToken, TestData.BunchId, parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model?.Id, Is.EqualTo(TestData.CashgameId));
        Assert.That(result.Model?.IsRunning, Is.EqualTo(true));
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
        Assert.That(result1.Model?.Count(), Is.EqualTo(1));
        Assert.That(result1.Model?.First().Id, Is.EqualTo(TestData.CashgameId));
    }

    [Test]
    [Order(6)]
    public async Task GetRunningCashgame()
    {
        var result1 = await TestClient.Cashgame.Get(TestData.UserToken, TestData.CashgameId);
        Assert.That(result1.Model, Is.Not.Null);
        Assert.That(result1.Model?.Id, Is.EqualTo("1"));
        Assert.That(result1.Model?.IsRunning, Is.True);
        Assert.That(result1.Model?.Players.Count, Is.EqualTo(3));

        Assert.That(result1.Model?.Players[0].Name, Is.EqualTo("Player Name"));
        Assert.That(result1.Model?.Players[0].Actions[0].Type, Is.EqualTo("buyin"));
        Assert.That(result1.Model?.Players[0].Actions[0].Added, Is.EqualTo(100));
        Assert.That(result1.Model?.Players[0].Actions[1].Type, Is.EqualTo("report"));
        Assert.That(result1.Model?.Players[0].Actions[1].Stack, Is.EqualTo(175));

        Assert.That(result1.Model?.Players[1].Name, Is.EqualTo("User"));
        Assert.That(result1.Model?.Players[1].Actions[0].Type, Is.EqualTo("buyin"));
        Assert.That(result1.Model?.Players[1].Actions[0].Added, Is.EqualTo(200));
        Assert.That(result1.Model?.Players[1].Actions[1].Type, Is.EqualTo("report"));
        Assert.That(result1.Model?.Players[1].Actions[1].Stack, Is.EqualTo(265));

        Assert.That(result1.Model?.Players[2].Name, Is.EqualTo("Manager"));
        Assert.That(result1.Model?.Players[2].Actions[0].Type, Is.EqualTo("buyin"));
        Assert.That(result1.Model?.Players[2].Actions[0].Added, Is.EqualTo(100));
        Assert.That(result1.Model?.Players[2].Actions[1].Type, Is.EqualTo("buyin"));
        Assert.That(result1.Model?.Players[2].Actions[1].Added, Is.EqualTo(100));
        Assert.That(result1.Model?.Players[2].Actions[1].Stack, Is.EqualTo(150));
        Assert.That(result1.Model?.Players[2].Actions[2].Type, Is.EqualTo("report"));
        Assert.That(result1.Model?.Players[2].Actions[2].Stack, Is.EqualTo(75));
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
        Assert.That(result.Model?.Id, Is.EqualTo("1"));
        Assert.That(result.Model?.IsRunning, Is.False);
        Assert.That(result.Model?.Players.Count, Is.EqualTo(3));

        Assert.That(result.Model?.Players[0].Name, Is.EqualTo("Player Name"));
        Assert.That(result.Model?.Players[0].Actions[2].Type, Is.EqualTo("cashout"));
        Assert.That(result.Model?.Players[0].Actions[2].Stack, Is.EqualTo(310));

        Assert.That(result.Model?.Players[1].Name, Is.EqualTo("User"));
        Assert.That(result.Model?.Players[1].Actions[2].Type, Is.EqualTo("cashout"));
        Assert.That(result.Model?.Players[1].Actions[2].Stack, Is.EqualTo(255));

        Assert.That(result.Model?.Players[2].Name, Is.EqualTo("Manager"));
        Assert.That(result.Model?.Players[2].Actions[3].Type, Is.EqualTo("cashout"));
        Assert.That(result.Model?.Players[2].Actions[3].Stack, Is.EqualTo(85));
    }

    [Test]
    [Order(9)]
    public async Task AddCashgameToEvent()
    {
        var parameters = new UpdateCashgamePostModel(TestData.BunchLocationId, TestData.EventId);
        var result = await TestClient.Cashgame.Update(TestData.ManagerToken, TestData.CashgameId, parameters);
        Assert.That(result.Success, Is.True);
        Assert.That(result.Model?.Event?.Name, Is.EqualTo(TestData.EventName));
    }

    [Test]
    [Order(10)]
    public async Task ListCashgamesByBunch()
    {
        var result = await TestClient.Cashgame.ListByBunch(TestData.UserToken, TestData.BunchId);
        Assert.That(result.Success, Is.True);

        var list = result.Model?.ToList();
        Assert.That(list?.Count, Is.EqualTo(1));
    }

    [Test]
    [Order(11)]
    public async Task ListCashgamesByBunchAndYear()
    {
        var result = await TestClient.Cashgame.ListByBunch(TestData.UserToken, TestData.BunchId, DateTime.Now.Year);
        Assert.That(result.Success, Is.True);

        var list = result.Model?.ToList();
        Assert.That(list?.Count, Is.EqualTo(1));
    }

    [Test]
    [Order(12)]
    public async Task ListCashgamesByEvent()
    {
        var result = await TestClient.Cashgame.ListByEvent(TestData.UserToken, TestData.EventId);
        Assert.That(result.Success, Is.True);

        var list = result.Model?.ToList();
        Assert.That(list?.Count, Is.EqualTo(1));
    }

    [Test]
    [Order(12)]
    public async Task ListCashgamesByPlayer()
    {
        var result = await TestClient.Cashgame.ListByPlayer(TestData.UserToken, TestData.PlayerPlayerId);
        Assert.That(result.Success, Is.True);

        var list = result.Model?.ToList();
        Assert.That(list?.Count, Is.EqualTo(1));
    }

    [Test]
    [Order(13)]
    public async Task DeleteCashgameInEvent_Fails()
    {
        var deleteResult = await TestClient.Cashgame.Delete(TestData.ManagerToken, TestData.CashgameId);
        Assert.That(deleteResult.Success, Is.False);
    }

    [Test]
    [Order(14)]
    public async Task RemoveCashgameFromEvent()
    {
        var parameters = new UpdateCashgamePostModel(TestData.BunchLocationId, null);
        var result = await TestClient.Cashgame.Update(TestData.ManagerToken, TestData.CashgameId, parameters);
        Assert.That(result.Success, Is.True);
        Assert.That(result.Model?.Event, Is.Null);
    }

    [Test]
    [Order(15)]
    public async Task DeleteCashgameWithResults_Fails()
    {
        var deleteResult = await TestClient.Cashgame.Delete(TestData.ManagerToken, TestData.CashgameId);
        Assert.That(deleteResult.Success, Is.False);
    }

    [Test]
    [Order(16)]
    public async Task ClearCashgameBeforeDeleting_Succeeds()
    {
        var cashgameResult = await TestClient.Cashgame.Get(TestData.ManagerToken, TestData.CashgameId);
        var cashgame = cashgameResult.Model;
        if (cashgame == null)
            return;

        foreach (var player in cashgame.Players)
        {
            foreach (var action in player.Actions.Reverse())
            {
                await TestClient.Action.Delete(TestData.ManagerToken, TestData.CashgameId, action.Id);
            }
        }

        var deleteResult = await TestClient.Cashgame.Delete(TestData.ManagerToken, TestData.CashgameId);
        Assert.That(deleteResult.Success, Is.True);

        var getResult = await TestClient.Cashgame.ListByBunch(TestData.ManagerToken, TestData.BunchId);
        var list = getResult.Model?.ToList();
        Assert.That(list?.Count, Is.EqualTo(0));
    }

    private async Task Buyin(string? token, string cashgameId, string playerId, int buyin, int leftInStack = 0)
    {
        var parameters = new AddCashgameActionPostModel("buyin", playerId, buyin, leftInStack);
        var result = await TestClient.Action.Add(token, cashgameId, parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task Report(string? token, string cashgameId, string playerId, int stack)
    {
        var parameters = new AddCashgameActionPostModel("report", playerId, 0, stack);
        var result = await TestClient.Action.Add(token, cashgameId, parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task Cashout(string? token, string cashgameId, string playerId, int stack)
    {
        var parameters = new AddCashgameActionPostModel("cashout", playerId, 0, stack);
        var result = await TestClient.Action.Add(token, cashgameId, parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task Update(string? token, string cashgameId, string actionId, int stack)
    {
        var parameters = new UpdateActionPostModel(DateTime.UtcNow, stack, null);
        var result = await TestClient.Action.Update(token, cashgameId, actionId, parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task Delete(string? token, string cashgameId, string actionId)
    {
        var result = await TestClient.Action.Delete(token, cashgameId, actionId);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}