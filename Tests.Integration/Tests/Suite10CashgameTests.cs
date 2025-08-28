using System.Net;
using Api.Models.CashgameModels;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.Cashgame)]
public class Suite10CashgameTests
{
    [Test]
    [Order(1)]
    public async Task Test01AddCashgame()
    {
        var token = await LoginHelper.GetUserToken();
        var parameters = new AddCashgamePostModel(TestData.BunchLocationId);
        var result = await TestClient.Cashgame.Add(token, TestData.BunchId, parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model?.Id, Is.EqualTo(TestData.CashgameId));
        Assert.That(result.Model?.IsRunning, Is.EqualTo(true));
    }

    [Test]
    [Order(2)]
    public async Task Test02Buyin()
    {
        var managerToken = await LoginHelper.GetAdminToken();
        var userToken = await LoginHelper.GetAdminToken();
        await Buyin(managerToken, TestData.CashgameId, TestData.ManagerPlayerId, 100);
        await Buyin(userToken, TestData.CashgameId, TestData.UserPlayerId, 200);
        await Buyin(managerToken, TestData.CashgameId, TestData.PlayerPlayerId, 100);
        await Buyin(managerToken, TestData.CashgameId, TestData.ManagerPlayerId, 100, 50);
    }

    [Test]
    [Order(3)]
    public async Task Test03Report()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var userToken = await LoginHelper.GetUserToken();
        await Report(managerToken, TestData.CashgameId, TestData.ManagerPlayerId, 75);
        await Report(userToken, TestData.CashgameId, TestData.UserPlayerId, 265);
        await Report(managerToken, TestData.CashgameId, TestData.PlayerPlayerId, 175);
    }

    [Test]
    [Order(4)]
    public async Task Test04AddUpdateAndDeleteAction()
    {
        const string actionId = "8";
        var managerToken = await LoginHelper.GetManagerToken();
        await Report(managerToken, TestData.CashgameId, TestData.ManagerPlayerId, 5000);
        await Update(managerToken, TestData.CashgameId, actionId, 5001);
        await Delete(managerToken, TestData.CashgameId, actionId);
    }

    [Test]
    [Order(5)]
    public async Task Test05GetCurrentCashgameId()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result1 = await TestClient.Cashgame.Current(userToken, TestData.BunchId);
        Assert.That(result1.Model, Is.Not.Null);
        Assert.That(result1.Model?.Count(), Is.EqualTo(1));
        Assert.That(result1.Model?.First().Id, Is.EqualTo(TestData.CashgameId));
    }

    [Test]
    [Order(6)]
    public async Task Test06GetRunningCashgame()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result1 = await TestClient.Cashgame.Get(userToken, TestData.CashgameId);
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
    public async Task Test07Cashout()
    {
        var userToken = await LoginHelper.GetUserToken();
        var managerToken = await LoginHelper.GetManagerToken();
        await Cashout(userToken, TestData.CashgameId, TestData.UserPlayerId, 255);
        await Cashout(managerToken, TestData.CashgameId, TestData.ManagerPlayerId, 85);
        await Cashout(managerToken, TestData.CashgameId, TestData.PlayerPlayerId, 310);
    }

    [Test]
    [Order(8)]
    public async Task Test08GetFinishedCashgame()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result = await TestClient.Cashgame.Get(userToken, TestData.CashgameId);
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
    public async Task Test09AddCashgameToEvent()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var parameters = new UpdateCashgamePostModel(TestData.BunchLocationId, TestData.EventId);
        var result = await TestClient.Cashgame.Update(managerToken, TestData.CashgameId, parameters);
        Assert.That(result.Success, Is.True);
        Assert.That(result.Model?.Event?.Name, Is.EqualTo(TestData.EventName));
    }

    [Test]
    [Order(10)]
    public async Task Test10ListCashgamesByBunch()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result = await TestClient.Cashgame.ListByBunch(userToken, TestData.BunchId);
        Assert.That(result.Success, Is.True);

        var list = result.Model?.ToList();
        Assert.That(list?.Count, Is.EqualTo(1));
    }

    [Test]
    [Order(11)]
    public async Task Test11ListCashgamesByBunchAndYear()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result = await TestClient.Cashgame.ListByBunch(userToken, TestData.BunchId, DateTime.Now.Year);
        Assert.That(result.Success, Is.True);

        var list = result.Model?.ToList();
        Assert.That(list?.Count, Is.EqualTo(1));
    }

    [Test]
    [Order(12)]
    public async Task Test12ListCashgamesByEvent()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result = await TestClient.Cashgame.ListByEvent(userToken, TestData.EventId);
        Assert.That(result.Success, Is.True);

        var list = result.Model?.ToList();
        Assert.That(list?.Count, Is.EqualTo(1));
    }

    [Test]
    [Order(13)]
    public async Task Test13ListCashgamesByPlayer()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result = await TestClient.Cashgame.ListByPlayer(userToken, TestData.PlayerPlayerId);
        Assert.That(result.Success, Is.True);

        var list = result.Model?.ToList();
        Assert.That(list?.Count, Is.EqualTo(1));
    }

    [Test]
    [Order(14)]
    public async Task Test14DeleteCashgameInEvent_Fails()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var deleteResult = await TestClient.Cashgame.Delete(managerToken, TestData.CashgameId);
        Assert.That(deleteResult.Success, Is.False);
    }

    [Test]
    [Order(15)]
    public async Task Test15RemoveCashgameFromEvent()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var parameters = new UpdateCashgamePostModel(TestData.BunchLocationId, null);
        var result = await TestClient.Cashgame.Update(managerToken, TestData.CashgameId, parameters);
        Assert.That(result.Success, Is.True);
        Assert.That(result.Model?.Event, Is.Null);
    }

    [Test]
    [Order(16)]
    public async Task Test16DeleteCashgameWithResults_Fails()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var deleteResult = await TestClient.Cashgame.Delete(managerToken, TestData.CashgameId);
        Assert.That(deleteResult.Success, Is.False);
    }

    [Test]
    [Order(17)]
    public async Task Test17ClearCashgameBeforeDeleting_Succeeds()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var cashgameResult = await TestClient.Cashgame.Get(managerToken, TestData.CashgameId);
        var cashgame = cashgameResult.Model;
        if (cashgame == null)
            return;

        foreach (var player in cashgame.Players)
        {
            foreach (var action in player.Actions.Reverse())
            {
                await TestClient.Action.Delete(managerToken, TestData.CashgameId, action.Id);
            }
        }

        var deleteResult = await TestClient.Cashgame.Delete(managerToken, TestData.CashgameId);
        Assert.That(deleteResult.Success, Is.True);

        var getResult = await TestClient.Cashgame.ListByBunch(managerToken, TestData.BunchId);
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