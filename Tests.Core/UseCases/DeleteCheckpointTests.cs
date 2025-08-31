using Core.Entities;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class DeleteCheckpointTests : TestBase
{
    [Test]
    public async Task DeleteCheckpoint_EndedGame_DeletesCheckpointAndReturnsCorrectValues()
    {
        var currentBunch = new CurrentBunch(TestData.BunchA.Id, TestData.BunchA.Slug);
        var request = new DeleteCheckpoint.Request(
            new PrincipalInTest(canDeleteCheckpoint: true, currentBunch: currentBunch),
            TestData.ReportCheckpointId);
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

        var currentBunch = new CurrentBunch(TestData.BunchA.Id, TestData.BunchA.Slug);
        var request =
            new DeleteCheckpoint.Request(new PrincipalInTest(canDeleteCheckpoint: true, currentBunch: currentBunch),
                "12");
        var result = await Sut.Execute(request);

        var deletedCheckpointIds = Deps.Cashgame.Updated?.DeletedCheckpoints.Select(o => o.Id);
        Assert.That(deletedCheckpointIds?.Contains("12"), Is.True);
        Assert.That(result.Data?.Slug, Is.EqualTo("bunch-a"));
        Assert.That(result.Data?.CashgameId, Is.EqualTo("3"));
        Assert.That(result.Data?.GameIsRunning, Is.True);
    }

    private DeleteCheckpoint Sut => new(Deps.Cashgame);
}