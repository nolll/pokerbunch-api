using Core.Entities;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class DeleteCheckpointTests : TestBase
{
    private readonly ICashgameRepository _cashgameRepository = Substitute.For<ICashgameRepository>();

    [Xunit.Theory]
    [InlineData(GameStatus.Finished)]
    [InlineData(GameStatus.Running)]
    public async Task DeleteCheckpoint_EndedGame_DeletesCheckpointAndReturnsCorrectValues(GameStatus status)
    {
        var cashgame = Create.Cashgame(status: status);
        var buyin = Create.BuyinAction(cashgameId: cashgame.Id);
        var report = Create.ReportAction(cashgameId: cashgame.Id);
        cashgame.SetCheckpoints([buyin, report]);
        _cashgameRepository.GetByCheckpoint(report.Id).Returns(cashgame);
        
        var userBunch = Create.UserBunch(TestData.BunchA.Id, TestData.BunchA.Slug);
        var request = new DeleteCheckpoint.Request(
            new AuthInTest(canDeleteCheckpoint: true, userBunch: userBunch),
            report.Id);
        var result = await Sut.Execute(request);

        await _cashgameRepository.Received()
            .Update(Arg.Is<Cashgame>(o => o.DeletedCheckpoints.Any(c => c.Id == report.Id)));
        
        result.Data!.Slug.Should().Be("bunch-a");
        result.Data!.CashgameId.Should().Be(cashgame.Id);
        result.Data!.GameIsRunning.Should().Be(status == GameStatus.Running);
    }

    private DeleteCheckpoint Sut => new(_cashgameRepository);
}