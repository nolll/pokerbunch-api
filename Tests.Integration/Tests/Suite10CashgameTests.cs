using System.Net;
using Api.Models.CashgameModels;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    [Fact]
    [Order(TestSuite.Cashgame, 1)]
    public async Task Suite10Cashgame_01AddCashgame()
    {
        var token = await LoginHelper.GetUserToken();
        var parameters = new AddCashgamePostModel(Data.BunchLocationId);
        var result = await ApiClient.Cashgame.Add(token, Data.BunchId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Id.Should().Be(Data.CashgameId);
        result.Model.IsRunning.Should().BeTrue();
    }

    [Fact]
    [Order(TestSuite.Cashgame, 2)]
    public async Task Suite10Cashgame_02Buyin()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var userToken = await LoginHelper.GetUserToken();
        await Buyin(managerToken, Data.CashgameId, Data.ManagerPlayerId, 100);
        await Buyin(userToken, Data.CashgameId, Data.UserPlayerId, 200);
        await Buyin(managerToken, Data.CashgameId, Data.PlayerPlayerId, 100);
        await Buyin(managerToken, Data.CashgameId, Data.ManagerPlayerId, 100, 50);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 3)]
    public async Task Suite10Cashgame_03Report()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var userToken = await LoginHelper.GetUserToken();
        await Report(managerToken, Data.CashgameId, Data.ManagerPlayerId, 75);
        await Report(userToken, Data.CashgameId, Data.UserPlayerId, 265);
        await Report(managerToken, Data.CashgameId, Data.PlayerPlayerId, 175);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 4)]
    public async Task Suite10Cashgame_04AddUpdateAndDeleteAction()
    {
        const string actionId = "8";
        var managerToken = await LoginHelper.GetManagerToken();
        await Report(managerToken, Data.CashgameId, Data.ManagerPlayerId, 5000);
        await Update(managerToken, Data.CashgameId, actionId, 5001);
        await Delete(managerToken, Data.CashgameId, actionId);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 5)]
    public async Task Suite10Cashgame_05GetCurrentCashgameId()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result1 = await ApiClient.Cashgame.Current(userToken, Data.BunchId);
        result1.Model.Should().NotBeNull();
        result1.Model.Count().Should().Be(1);
        result1.Model.First().Id.Should().Be(Data.CashgameId);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 6)]
    public async Task Suite10Cashgame_06GetRunningCashgame()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result1 = await ApiClient.Cashgame.Get(userToken, Data.CashgameId);
        result1.Model.Should().NotBeNull();
        result1.Model.Id.Should().Be("1");
        result1.Model.IsRunning.Should().BeTrue();
        result1.Model.Players.Count.Should().Be(3);

        result1.Model.Players[0].Name.Should().Be(Data.PlayerName);
        result1.Model.Players[0].Actions[0].Type.Should().Be("buyin");
        result1.Model.Players[0].Actions[0].Added.Should().Be(100);
        result1.Model.Players[0].Actions[1].Type.Should().Be("report");
        result1.Model.Players[0].Actions[1].Stack.Should().Be(175);

        result1.Model.Players[1].Name.Should().Be(Data.UserDisplayName);
        result1.Model.Players[1].Actions[0].Type.Should().Be("buyin");
        result1.Model.Players[1].Actions[0].Added.Should().Be(200);
        result1.Model.Players[1].Actions[1].Type.Should().Be("report");
        result1.Model.Players[1].Actions[1].Stack.Should().Be(265);

        result1.Model.Players[2].Name.Should().Be(Data.ManagerDisplayName);
        result1.Model.Players[2].Actions[0].Type.Should().Be("buyin");
        result1.Model.Players[2].Actions[0].Added.Should().Be(100);
        result1.Model.Players[2].Actions[1].Type.Should().Be("buyin");
        result1.Model.Players[2].Actions[1].Added.Should().Be(100);
        result1.Model.Players[2].Actions[1].Stack.Should().Be(150);
        result1.Model.Players[2].Actions[2].Type.Should().Be("report");
        result1.Model.Players[2].Actions[2].Stack.Should().Be(75);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 7)]
    public async Task Suite10Cashgame_07Cashout()
    {
        var userToken = await LoginHelper.GetUserToken();
        var managerToken = await LoginHelper.GetManagerToken();
        await Cashout(userToken, Data.CashgameId, Data.UserPlayerId, 255);
        await Cashout(managerToken, Data.CashgameId, Data.ManagerPlayerId, 85);
        await Cashout(managerToken, Data.CashgameId, Data.PlayerPlayerId, 310);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 8)]
    public async Task Suite10Cashgame_08GetFinishedCashgame()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result = await ApiClient.Cashgame.Get(userToken, Data.CashgameId);
        result.Model.Should().NotBeNull();
        result.Model.Id.Should().Be("1");
        result.Model.IsRunning.Should().BeFalse();
        result.Model.Players.Count.Should().Be(3);

        result.Model.Players[0].Name.Should().Be("Player Name");
        result.Model.Players[0].Actions[2].Type.Should().Be("cashout");
        result.Model.Players[0].Actions[2].Stack.Should().Be(310);

        result.Model.Players[1].Name.Should().Be("User");
        result.Model.Players[1].Actions[2].Type.Should().Be("cashout");
        result.Model.Players[1].Actions[2].Stack.Should().Be(255);

        result.Model.Players[2].Name.Should().Be("Manager");
        result.Model.Players[2].Actions[3].Type.Should().Be("cashout");
        result.Model.Players[2].Actions[3].Stack.Should().Be(85);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 9)]
    public async Task Suite10Cashgame_09AddCashgameToEvent()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var parameters = new UpdateCashgamePostModel(Data.BunchLocationId, Data.EventId);
        var result = await ApiClient.Cashgame.Update(managerToken, Data.CashgameId, parameters);
        result.Success.Should().BeTrue();
        result.Model!.Event!.Name.Should().Be(Data.EventName);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 10)]
    public async Task Suite10Cashgame_10ListCashgamesByBunch()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result = await ApiClient.Cashgame.ListByBunch(userToken, Data.BunchId);
        result.Success.Should().BeTrue();

        var list = result.Model!.ToList();
        list.Count.Should().Be(1);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 11)]
    public async Task Suite10Cashgame_11ListCashgamesByBunchAndYear()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result = await ApiClient.Cashgame.ListByBunch(userToken, Data.BunchId, DateTime.Now.Year);
        result.Success.Should().BeTrue();

        var list = result.Model!.ToList();
        list.Count.Should().Be(1);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 12)]
    public async Task Suite10Cashgame_12ListCashgamesByEvent()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result = await ApiClient.Cashgame.ListByEvent(userToken, Data.EventId);
        result.Success.Should().BeTrue();

        var list = result.Model!.ToList();
        list.Count.Should().Be(1);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 13)]
    public async Task Suite10Cashgame_13ListCashgamesByPlayer()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result = await ApiClient.Cashgame.ListByPlayer(userToken, Data.PlayerPlayerId);
        result.Success.Should().BeTrue();

        var list = result.Model!.ToList();
        list.Count.Should().Be(1);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 14)]
    public async Task Suite10Cashgame_14DeleteCashgameInEvent_Fails()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var deleteResult = await ApiClient.Cashgame.Delete(managerToken, Data.CashgameId);
        deleteResult.Success.Should().BeFalse();
    }

    [Fact]
    [Order(TestSuite.Cashgame, 15)]
    public async Task Suite10Cashgame_15RemoveCashgameFromEvent()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var parameters = new UpdateCashgamePostModel(Data.BunchLocationId, null);
        var result = await ApiClient.Cashgame.Update(managerToken, Data.CashgameId, parameters);
        result.Success.Should().BeTrue();
        result.Model!.Event.Should().BeNull();
    }

    [Fact]
    [Order(TestSuite.Cashgame, 16)]
    public async Task Suite10Cashgame_16DeleteCashgameWithResults_Fails()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var deleteResult = await ApiClient.Cashgame.Delete(managerToken, Data.CashgameId);
        deleteResult.Success.Should().BeFalse();
    }

    [Fact]
    [Order(TestSuite.Cashgame, 17)]
    public async Task Suite10Cashgame_17ClearCashgameBeforeDeleting_Succeeds()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var cashgameResult = await ApiClient.Cashgame.Get(managerToken, Data.CashgameId);
        var cashgame = cashgameResult.Model;
        if (cashgame == null)
            return;

        foreach (var player in cashgame.Players)
        {
            foreach (var action in player.Actions.Reverse())
            {
                await ApiClient.Action.Delete(managerToken, Data.CashgameId, action.Id);
            }
        }

        var deleteResult = await ApiClient.Cashgame.Delete(managerToken, Data.CashgameId);
        deleteResult.Success.Should().BeTrue();

        var getResult = await ApiClient.Cashgame.ListByBunch(managerToken, Data.BunchId);
        var list = getResult.Model!.ToList();
        list.Count.Should().Be(0);
    }

    private async Task Buyin(string? token, string cashgameId, string playerId, int buyin, int leftInStack = 0)
    {
        var parameters = new AddCashgameActionPostModel("buyin", playerId, buyin, leftInStack);
        var result = await ApiClient.Action.Add(token, cashgameId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task Report(string? token, string cashgameId, string playerId, int stack)
    {
        var parameters = new AddCashgameActionPostModel("report", playerId, 0, stack);
        var result = await ApiClient.Action.Add(token, cashgameId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task Cashout(string? token, string cashgameId, string playerId, int stack)
    {
        var parameters = new AddCashgameActionPostModel("cashout", playerId, 0, stack);
        var result = await ApiClient.Action.Add(token, cashgameId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task Update(string? token, string cashgameId, string actionId, int stack)
    {
        var parameters = new UpdateActionPostModel(DateTime.UtcNow, stack, null);
        var result = await ApiClient.Action.Update(token, cashgameId, actionId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task Delete(string? token, string cashgameId, string actionId)
    {
        var result = await ApiClient.Action.Delete(token, cashgameId, actionId);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}