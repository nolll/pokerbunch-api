using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

public class DeleteCheckpointTests : TestBase
{
    [Test]
    public async Task DeleteCheckpoint_EndedGame_DeletesCheckpointAndReturnsCorrectValues()
    {
        var request = new DeleteCheckpoint.Request(TestData.ManagerUser.UserName, TestData.ReportCheckpointId);
        var result = await Sut.Execute(request);

        var deletedCheckpointIds = Deps.Cashgame.Updated?.DeletedCheckpoints.Select(o => o.Id);
        Assert.That(deletedCheckpointIds?.Contains(TestData.ReportCheckpointId), Is.True);
        Assert.That(result.Data?.Slug, Is.EqualTo("bunch-a"));
        Assert.That(result.Data?.CashgameId, Is.EqualTo("1"));
        Assert.That(result.Data?.GameIsRunning, Is.False);
    }

    [Test]
    public async Task DeleteCheckpoint_RunningGame_DeletesCheckpointAndReturnsCorrectValues()
    {
        Deps.Cashgame.SetupRunningGame();

        var request = new DeleteCheckpoint.Request(TestData.ManagerUser.UserName, "12");
        var result = await Sut.Execute(request);

        var deletedCheckpointIds = Deps.Cashgame.Updated?.DeletedCheckpoints.Select(o => o.Id);
        Assert.That(deletedCheckpointIds?.Contains("12"), Is.True);
        Assert.That(result.Data?.Slug, Is.EqualTo("bunch-a"));
        Assert.That(result.Data?.CashgameId, Is.EqualTo("3"));
        Assert.That(result.Data?.GameIsRunning, Is.True);
    }

    private DeleteCheckpoint Sut => new(
        Deps.Bunch,
        Deps.Cashgame,
        Deps.User,
        Deps.Player);
}