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
            new AuthInTest(canDeleteCheckpoint: true, currentBunch: currentBunch),
            TestData.ReportCheckpointId);
        var result = await Sut.Execute(request);

        var deletedCheckpointIds = Deps.Cashgame.Updated?.DeletedCheckpoints.Select(o => o.Id);
        deletedCheckpointIds?.Contains(TestData.ReportCheckpointId).Should().BeTrue();
        result.Data?.Slug.Should().Be("bunch-a");
        result.Data?.CashgameId.Should().Be("1");
        result.Data?.GameIsRunning.Should().BeFalse();
    }

    [Test]
    public async Task DeleteCheckpoint_RunningGame_DeletesCheckpointAndReturnsCorrectValues()
    {
        Deps.Cashgame.SetupRunningGame();

        var currentBunch = new CurrentBunch(TestData.BunchA.Id, TestData.BunchA.Slug);
        var request =
            new DeleteCheckpoint.Request(new AuthInTest(canDeleteCheckpoint: true, currentBunch: currentBunch),
                "12");
        var result = await Sut.Execute(request);

        var deletedCheckpointIds = Deps.Cashgame.Updated?.DeletedCheckpoints.Select(o => o.Id);
        deletedCheckpointIds?.Contains("12").Should().BeTrue();
        result.Data?.Slug.Should().Be("bunch-a");
        result.Data?.CashgameId.Should().Be("3");
        result.Data?.GameIsRunning.Should().BeTrue();
    }

    private DeleteCheckpoint Sut => new(Deps.Cashgame);
}