using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class CurrentCashgamesTests : TestBase
{
    private readonly ICashgameRepository _cashgameRepository = Substitute.For<ICashgameRepository>();
    
    [Fact]
    public async Task NoAccess_ReturnsAccessDeniedError()
    {
        var bunch = Create.Bunch();

        var request = CreateRequest(bunch, false);
        var result = await Sut.Execute(request);
        
        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }
    
    [Fact]
    public async Task HasAccess_WithGames_ReturnsListOfGames()
    {
        var cashgame = Create.Cashgame();
        var bunch = Create.Bunch();
        
        _cashgameRepository.GetRunning(bunch.Id).Returns(cashgame);
        
        var request = CreateRequest(bunch);
        var result = await Sut.Execute(request);
        
        var games = result.Data!.Games;
        games.Count.Should().Be(1);
        var game = games.First();
        game.Slug.Should().Be(bunch.Slug);
        game.Id.Should().Be(cashgame.Id);
    }

    [Fact]
    public async Task HasAccess_NoGames_ReturnsEmptyList()
    {
        var bunch = Create.Bunch();
        
        var request = CreateRequest(bunch);
        var result = await Sut.Execute(request);

        result.Data!.Games.Count.Should().Be(0);
    }
    
    private CurrentCashgames.Request CreateRequest(Bunch bunch, bool? canListCurrentGames = null)
    {
        var userBunch = Create.UserBunch(bunch.Id, bunch.Slug);
        var auth = new AuthInTest(
            canListCurrentGames: canListCurrentGames ?? true, 
            userBunch: userBunch);
        return new CurrentCashgames.Request(auth, bunch.Slug);
    }

    private CurrentCashgames Sut => new(_cashgameRepository);
}