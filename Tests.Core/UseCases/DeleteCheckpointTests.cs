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

        var deletedCheckpointIds = Deps.Cashgame.Updated.DeletedCheckpoints.Select(o => o.Id);
        Assert.IsTrue(deletedCheckpointIds.Contains(TestData.ReportCheckpointId));
        Assert.AreEqual("bunch-a", result.Data.Slug);
        Assert.AreEqual(1, result.Data.CashgameId);
        Assert.IsFalse(result.Data.GameIsRunning);
    }

    [Test]
    public async Task DeleteCheckpoint_RunningGame_DeletesCheckpointAndReturnsCorrectValues()
    {
        Deps.Cashgame.SetupRunningGame();

        var request = new DeleteCheckpoint.Request(TestData.ManagerUser.UserName, 12);
        var result = await Sut.Execute(request);

        var deletedCheckpointIds = Deps.Cashgame.Updated.DeletedCheckpoints.Select(o => o.Id);
        Assert.IsTrue(deletedCheckpointIds.Contains(12));
        Assert.AreEqual("bunch-a", result.Data.Slug);
        Assert.AreEqual(3, result.Data.CashgameId);
        Assert.IsTrue(result.Data.GameIsRunning);
    }

    private DeleteCheckpoint Sut => new(
        Deps.Bunch,
        Deps.Cashgame,
        Deps.User,
        Deps.Player);
}