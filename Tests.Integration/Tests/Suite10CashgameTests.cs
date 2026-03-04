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
        var user = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(user);
        var location = await bunch.AddLocation();
        
        var parameters = new AddCashgamePostModel(location.Id);
        var result = await ApiClient.Cashgame.Add(user.Token, bunch.Id, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.IsRunning.Should().BeTrue();
    }

    [Fact]
    [Order(TestSuite.Cashgame, 2)]
    public async Task Suite10Cashgame_02Buyin()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player = await bunch.AddPlayer();
        var location = await bunch.AddLocation();
        var cashgame = await bunch.StartCashgame(location);
        
        await Buyin(manager.Token, cashgame.Id, player.Id);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 3)]
    public async Task Suite10Cashgame_03Report()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player = await bunch.AddPlayer();
        var location = await bunch.AddLocation();
        var cashgame = await bunch.StartCashgame(location);
        
        await Buyin(manager.Token, cashgame.Id, player.Id);
        await Report(manager.Token, cashgame.Id, player.Id);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 4)]
    public async Task Suite10Cashgame_04AddUpdateAndDeleteAction()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player = await bunch.AddPlayer();
        var location = await bunch.AddLocation();
        var cashgame = await bunch.StartCashgame(location);

        var initialReport = DataFactory.Int();
        await Buyin(manager.Token, cashgame.Id, player.Id);
        await Report(manager.Token, cashgame.Id, player.Id, initialReport);

        var result = await ApiClient.Cashgame.Get(manager.Token, cashgame.Id);
        var report = result.Model!.Players.SelectMany(o => o.Actions).First(o => o.Type == ActionType.Report);
        report.Stack.Should().Be(initialReport);

        var updatedReport = DataFactory.Int();
        await Update(manager.Token, cashgame.Id, report.Id, updatedReport);
        
        result = await ApiClient.Cashgame.Get(manager.Token, cashgame.Id);
        report = result.Model!.Players.SelectMany(o => o.Actions).First(o => o.Type == ActionType.Report);
        report.Stack.Should().Be(updatedReport);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 5)]
    public async Task Suite10Cashgame_05GetCurrentCashgameId()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player = await bunch.AddPlayer();
        var location = await bunch.AddLocation();
        var cashgameFixture = await bunch.StartCashgame(location);
        await Buyin(manager.Token, cashgameFixture.Id, player.Id);
        
        var result1 = await ApiClient.Cashgame.Current(manager.Token, bunch.Id);
        result1.Model.Should().NotBeNull();
        result1.Model.Count().Should().Be(1);
        result1.Model.First().Id.Should().Be(cashgameFixture.Id);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 6)]
    public async Task Suite10Cashgame_06GetRunningCashgame()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player1 = await bunch.AddPlayer();
        var player2 = await bunch.AddPlayer();
        var location = await bunch.AddLocation();
        var cashgame = await bunch.StartCashgame(location);

        var p1Buyin = DataFactory.Int();
        var p2Buyin = DataFactory.Int();
        await Buyin(manager.Token, cashgame.Id, player1.Id, p1Buyin);
        await Buyin(manager.Token, cashgame.Id, player2.Id, p2Buyin);
        
        var p1Report = DataFactory.Int();
        var p2Report = DataFactory.Int();
        await Report(manager.Token, cashgame.Id, player1.Id, p1Report);
        await Report(manager.Token, cashgame.Id, player2.Id, p2Report);
        
        var result = await ApiClient.Cashgame.Get(manager.Token, cashgame.Id);
        result.Model.Should().NotBeNull();
        result.Model.Id.Should().Be(cashgame.Id);
        result.Model.IsRunning.Should().BeTrue();
        result.Model.Players.Count.Should().Be(2);

        var p1Results = result.Model.Players.First(o => o.Id == player1.Id);
        p1Results.Name.Should().Be(player1.Name);
        p1Results.Actions[0].Type.Should().Be(ActionType.Buyin);
        p1Results.Actions[0].Added.Should().Be(p1Buyin);
        p1Results.Actions[1].Type.Should().Be(ActionType.Report);
        p1Results.Actions[1].Stack.Should().Be(p1Report);

        var p2Results = result.Model.Players.First(o => o.Id == player2.Id);
        p2Results.Name.Should().Be(player2.Name);
        p2Results.Actions[0].Type.Should().Be(ActionType.Buyin);
        p2Results.Actions[0].Added.Should().Be(p2Buyin);
        p2Results.Actions[1].Type.Should().Be(ActionType.Report);
        p2Results.Actions[1].Stack.Should().Be(p2Report);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 7)]
    public async Task Suite10Cashgame_07Cashout()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player = await bunch.AddPlayer();
        var location = await bunch.AddLocation();
        var cashgame = await bunch.StartCashgame(location);
        
        await Buyin(manager.Token, cashgame.Id, player.Id);
        await Report(manager.Token, cashgame.Id, player.Id);
        await Cashout(manager.Token, cashgame.Id, player.Id);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 8)]
    public async Task Suite10Cashgame_08GetFinishedCashgame()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player1 = await bunch.AddPlayer();
        var player2 = await bunch.AddPlayer();
        var location = await bunch.AddLocation();
        var cashgame = await bunch.StartCashgame(location);

        var p1Cashout = DataFactory.Int();
        var p2Cashout = DataFactory.Int();
        await Buyin(manager.Token, cashgame.Id, player1.Id);
        await Buyin(manager.Token, cashgame.Id, player2.Id);
        await Report(manager.Token, cashgame.Id, player1.Id);
        await Report(manager.Token, cashgame.Id, player2.Id);
        await Cashout(manager.Token, cashgame.Id, player1.Id, p1Cashout);
        await Cashout(manager.Token, cashgame.Id, player2.Id, p2Cashout);
        
        var result = await ApiClient.Cashgame.Get(manager.Token, cashgame.Id);
        result.Model.Should().NotBeNull();
        result.Model.Id.Should().Be(cashgame.Id);
        result.Model.IsRunning.Should().BeFalse();
        result.Model.Players.Count.Should().Be(2);
        
        var p1Results = result.Model.Players.First(o => o.Id == player1.Id);
        p1Results.Name.Should().Be(player1.Name);
        p1Results.Actions.Last().Type.Should().Be(ActionType.Cashout);
        p1Results.Actions.Last().Stack.Should().Be(p1Cashout);

        var p2Results = result.Model.Players.First(o => o.Id == player2.Id);
        p2Results.Name.Should().Be(player2.Name);
        p2Results.Actions.Last().Type.Should().Be(ActionType.Cashout);
        p2Results.Actions.Last().Stack.Should().Be(p2Cashout);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 9)]
    public async Task Suite10Cashgame_09AddCashgameToEvent()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var location = await bunch.AddLocation();
        var @event = await bunch.AddEvent();
        var cashgame = await bunch.StartCashgame(location);
        
        var parameters = new UpdateCashgamePostModel(location.Id, @event.Id);
        var result = await ApiClient.Cashgame.Update(manager.Token, cashgame.Id, parameters);
        result.Success.Should().BeTrue();
        result.Model!.Event!.Name.Should().Be(@event.Name);
        
        var listResult = await ApiClient.Cashgame.ListByEvent(manager.Token, @event.Id);
        listResult.Success.Should().BeTrue();
        listResult.Model!.ToList().Count.Should().Be(1);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 10)]
    public async Task Suite10Cashgame_10ListCashgamesByBunch()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player1 = await bunch.AddPlayer();
        var player2 = await bunch.AddPlayer();
        var location = await bunch.AddLocation();
        var cashgame = await bunch.StartCashgame(location);
        await cashgame.Play(manager, player1, player2);
        
        var result = await ApiClient.Cashgame.ListByBunch(manager.Token, bunch.Id);
        result.Success.Should().BeTrue();

        var list = result.Model!.ToList();
        list.Count.Should().Be(1);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 11)]
    public async Task Suite10Cashgame_11ListCashgamesByBunchAndYear()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player1 = await bunch.AddPlayer();
        var player2 = await bunch.AddPlayer();
        var location = await bunch.AddLocation();
        var cashgame = await bunch.StartCashgame(location);
        await cashgame.Play(manager, player1, player2);
        
        var result = await ApiClient.Cashgame.ListByBunch(manager.Token, bunch.Id, DateTime.Now.Year);
        result.Success.Should().BeTrue();

        var list = result.Model!.ToList();
        list.Count.Should().Be(1);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 13)]
    public async Task Suite10Cashgame_13ListCashgamesByPlayer()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player1 = await bunch.AddPlayer();
        var player2 = await bunch.AddPlayer();
        var player3 = await bunch.AddPlayer();
        var location = await bunch.AddLocation();
        var cashgame1 = await bunch.StartCashgame(location);
        var cashgame2 = await bunch.StartCashgame(location);
        await cashgame1.Play(manager, player1, player2);
        await cashgame2.Play(manager, player2, player3);
        
        var result = await ApiClient.Cashgame.ListByPlayer(manager.Token, player1.Id);
        result.Success.Should().BeTrue();

        var list = result.Model!.ToList();
        list.Count.Should().Be(1);
    }

    [Fact]
    [Order(TestSuite.Cashgame, 14)]
    public async Task Suite10Cashgame_14DeleteCashgameInEvent_Fails()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player1 = await bunch.AddPlayer();
        var player2 = await bunch.AddPlayer();
        var location = await bunch.AddLocation();
        var @event = await bunch.AddEvent();
        var cashgame = await bunch.StartCashgame(location);
        await cashgame.AddToEvent(manager.Token, @event);
        await cashgame.Play(manager, player1, player2);
        
        var deleteResult = await ApiClient.Cashgame.Delete(manager.Token, cashgame.Id);
        deleteResult.Success.Should().BeFalse();
    }

    [Fact]
    [Order(TestSuite.Cashgame, 15)]
    public async Task Suite10Cashgame_15RemoveCashgameFromEvent()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player1 = await bunch.AddPlayer();
        var player2 = await bunch.AddPlayer();
        var location = await bunch.AddLocation();
        var @event = await bunch.AddEvent();
        var cashgame = await bunch.StartCashgame(location);
        await cashgame.AddToEvent(manager.Token, @event);
        await cashgame.Play(manager, player1, player2);
        
        var parameters = new UpdateCashgamePostModel(location.Id, null);
        var result = await ApiClient.Cashgame.Update(manager.Token, cashgame.Id, parameters);
        result.Success.Should().BeTrue();
        result.Model!.Event.Should().BeNull();
    }

    [Fact]
    [Order(TestSuite.Cashgame, 16)]
    public async Task Suite10Cashgame_16DeleteCashgameWithResults_Fails()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player1 = await bunch.AddPlayer();
        var player2 = await bunch.AddPlayer();
        var location = await bunch.AddLocation();
        var cashgame = await bunch.StartCashgame(location);
        await cashgame.Play(manager, player1, player2);
        
        var deleteResult = await ApiClient.Cashgame.Delete(manager.Token, cashgame.Id);
        deleteResult.Success.Should().BeFalse();
    }

    [Fact]
    [Order(TestSuite.Cashgame, 17)]
    public async Task Suite10Cashgame_17ClearCashgameBeforeDeleting_Succeeds()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player1 = await bunch.AddPlayer();
        var player2 = await bunch.AddPlayer();
        var location = await bunch.AddLocation();
        var cashgame = await bunch.StartCashgame(location);
        await cashgame.Play(manager, player1, player2);
        
        var cashgameResult = await ApiClient.Cashgame.Get(manager.Token, cashgame.Id);

        foreach (var player in cashgameResult.Model!.Players)
        {
            foreach (var action in player.Actions.Reverse())
            {
                await ApiClient.Action.Delete(manager.Token, cashgame.Id, action.Id);
            }
        }

        var deleteResult = await ApiClient.Cashgame.Delete(manager.Token, cashgame.Id);
        deleteResult.Success.Should().BeTrue();

        var getResult = await ApiClient.Cashgame.ListByBunch(manager.Token, bunch.Id);
        var list = getResult.Model!.ToList();
        list.Count.Should().Be(0);
    }

    private async Task Buyin(string? token, string cashgameId, string playerId, int? buyin = null, int leftInStack = 0)
    {
        var parameters = new AddCashgameActionPostModel(ActionType.Buyin, playerId, buyin ?? DataFactory.Int(), leftInStack);
        var result = await ApiClient.Action.Add(token, cashgameId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task Report(string? token, string cashgameId, string playerId, int? stack = null)
    {
        var parameters = new AddCashgameActionPostModel(ActionType.Report, playerId, 0, stack ?? DataFactory.Int());
        var result = await ApiClient.Action.Add(token, cashgameId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task Cashout(string? token, string cashgameId, string playerId, int? stack = null)
    {
        var parameters = new AddCashgameActionPostModel(ActionType.Cashout, playerId, 0, stack ?? DataFactory.Int());
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