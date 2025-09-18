using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class DeletePlayerTests : TestBase
{
    private readonly IPlayerRepository _playerRepository = Substitute.For<IPlayerRepository>();
    private readonly ICashgameRepository _cashgameRepository = Substitute.For<ICashgameRepository>();

    [Fact]
    public async Task DeletePlayer_PlayerHasntPlayed_PlayerDeletedAndReturnUrlIsPlayerIndex()
    {
        var bunch = Create.Bunch();
        var player = Create.Player(bunchId: bunch.Id);
        _playerRepository.Get(player.Id).Returns(player);
        _cashgameRepository.GetByPlayer(player.Id).Returns([]);

        var userBunch = Create.UserBunch(bunch.Id, bunch.Slug);
        var request = new DeletePlayer.Request(new AuthInTest(canDeletePlayer: true, userBunch: userBunch), player.Id);
        var result = await Sut.Execute(request);

        result.Success.Should().BeTrue();

        await _playerRepository.Received().Delete(player.Id);
    }
    
    [Fact]
    public async Task DeletePlayer_PlayerHasPlayed_ReturnsError()
    {
        var bunch = Create.Bunch();
        var player = Create.Player(bunchId: bunch.Id);
        var cashgame = Create.Cashgame();
        _playerRepository.Get(player.Id).Returns(player);
        _cashgameRepository.GetByPlayer(player.Id).Returns([cashgame]);

        var userBunch = Create.UserBunch(bunch.Id, bunch.Slug);
        var request = new DeletePlayer.Request(new AuthInTest(canDeletePlayer: true, userBunch: userBunch), player.Id);
        var result = await Sut.Execute(request);

        result.Success.Should().BeFalse();
        await _playerRepository.DidNotReceive().Delete(Arg.Any<string>());
    }

    private DeletePlayer Sut => new(
        _playerRepository,
        _cashgameRepository);
}