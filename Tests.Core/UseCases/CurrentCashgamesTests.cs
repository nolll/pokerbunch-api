using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;
using Xunit;

namespace Tests.Core.UseCases;

public class CurrentCashgamesTests : TestBase
{
    private readonly ICashgameRepository _cashgameRepository = Substitute.For<ICashgameRepository>();
    
    [Fact]
    public async Task HasAccess_WithGames_ReturnsListOfGames()
    {
        var cashgame = CreateCashgame();
        var bunch = CreateBunch();
        
        _cashgameRepository.GetRunning(bunch.Id).Returns(cashgame);

        var result = await ExecuteAsync(true, bunch);
        
        var games = result.Data!.Games;
        games.Count.Should().Be(1);
        var game = games.First();
        game.Slug.Should().Be(bunch.Slug);
        game.Id.Should().Be(cashgame.Id);
    }

    [Fact]
    public async Task HasAccess_NoGames_ReturnsEmptyList()
    {
        var bunch = CreateBunch();

        var result = await ExecuteAsync(true, bunch);

        result.Data!.Games.Count.Should().Be(0);
    }

    [Fact]
    public async Task NoAccess_ReturnsAccessDeniedError()
    {
        var bunch = CreateBunch();

        var result = await ExecuteAsync(false, bunch);
        
        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }
    
    private async Task<UseCaseResult<CurrentCashgames.Result>> ExecuteAsync(bool canListCurrentGames, Bunch bunch)
    {
        var currentBunch = new CurrentBunch(bunch.Id, bunch.Slug);
        var auth = new AuthInTest(canListCurrentGames: canListCurrentGames, currentBunch: currentBunch);
        return await Sut.Execute(new CurrentCashgames.Request(auth, bunch.Slug));
    }

    private CurrentCashgames Sut => new(_cashgameRepository);
}