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

    [Theory]
    [InlineData(GameStatus.Finished)]
    [InlineData(GameStatus.Running)]
    public async Task DeleteCheckpoint_EndedGame_DeletesCheckpointAndReturnsCorrectValues(GameStatus status)
    {
        var bunch = Create.Bunch();
        var cashgame = Create.Cashgame(bunchSlug: bunch.Slug, status: status);
        var buyin = Create.BuyinAction(cashgameId: cashgame.Id);
        var report = Create.ReportAction(cashgameId: cashgame.Id);
        cashgame.SetCheckpoints([buyin, report]);
        _cashgameRepository.GetByCheckpoint(report.Id).Returns(cashgame);

        var request = CreateRequest(bunch.Id, bunch.Slug, report.Id);
        var result = await Sut.Execute(request);

        await _cashgameRepository.Received()
            .Update(Arg.Is<Cashgame>(o => o.DeletedCheckpoints.Any(c => c.Id == report.Id)));
        
        result.Data!.Slug.Should().Be(bunch.Slug);
        result.Data!.CashgameId.Should().Be(cashgame.Id);
        result.Data!.GameIsRunning.Should().Be(status == GameStatus.Running);
    }

    private DeleteCheckpoint.Request CreateRequest(string? bunchId = null, string? slug = null, string? reportId = null)
    {
        var userBunch = Create.UserBunch(bunchId, slug);
        return new DeleteCheckpoint.Request(
            new AuthInTest(canDeleteCheckpoint: true, userBunch: userBunch),
            reportId ?? Create.String());
    }

    private DeleteCheckpoint Sut => new(_cashgameRepository);
}