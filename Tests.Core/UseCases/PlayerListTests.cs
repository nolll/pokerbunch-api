using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class PlayerListTests : TestBase
{
    private readonly IBunchRepository _bunchRepository = Substitute.For<IBunchRepository>();
    private readonly IPlayerRepository _playerRepository = Substitute.For<IPlayerRepository>();

    [Fact]
    public async Task Execute_SlugAndPlayersAreSet()
    {
        var bunch = Create.Bunch();
        _bunchRepository.GetBySlug(bunch.Slug).Returns(bunch);
        var player1 = Create.Player(displayName: "a");
        var player2 = Create.Player(displayName: "b");
        _playerRepository.List(bunch.Id).Returns([player1, player2]);
        
        var request = CreateRequest(bunch.Slug);
        var result = await Sut.Execute(request);

        result.Data!.Slug.Should().Be(bunch.Slug);
        result.Data!.Players.Count.Should().Be(2);
        result.Data!.Players[0].Id.Should().Be(player1.Id);
        result.Data!.Players[0].Name.Should().Be(player1.DisplayName);
        result.Data!.Players[1].Id.Should().Be(player2.Id);
        result.Data!.Players[1].Name.Should().Be(player2.DisplayName);
        result.Data!.CanAddPlayer.Should().BeFalse();
    }

    [Fact]
    public async Task Execute_PlayerIsManager_CanAddPlayerIsTrue()
    {
        var bunch = Create.Bunch();
        _bunchRepository.GetBySlug(bunch.Slug).Returns(bunch);
        _playerRepository.List(bunch.Id).Returns([]);
        
        var request = CreateRequest(bunch.Slug, canAddPlayer: true);
        var result = await Sut.Execute(request);

        result.Data!.CanAddPlayer.Should().BeTrue();
    }
    
    private GetPlayerList.Request CreateRequest(string? slug = null, bool? canListPlayers = null, bool? canAddPlayer = null)
    {
        return new GetPlayerList.Request(
            new AuthInTest(
                canListPlayers: canListPlayers ?? true,
                canAddPlayer: canAddPlayer ?? false), 
            slug ?? Create.String());
    }

    private GetPlayerList Sut => new(
        _bunchRepository,
        _playerRepository);
}