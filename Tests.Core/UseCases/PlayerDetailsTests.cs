using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using NUnit.Framework;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class PlayerDetailsTests : TestBase
{
    private readonly IBunchRepository _bunchRepository = Substitute.For<IBunchRepository>();
    private readonly IPlayerRepository _playerRepository = Substitute.For<IPlayerRepository>();
    private readonly ICashgameRepository _cashgameRepository = Substitute.For<ICashgameRepository>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

    [Fact]
    public async Task PlayerDetails_NoAccess_ReturnsError()
    {
        var player = Create.Player();
        _playerRepository.Get(player.Id).Returns(player);
        var request = CreateRequest(playerId: player.Id, canSeePlayer: false);
        var result = await Sut.Execute(request);
        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }
    
    [Fact]
    public async Task PlayerDetails_WithoutUser_DisplayNameIsSet()
    {
        var bunch = Create.Bunch();
        _bunchRepository.Get(bunch.Id).Returns(bunch);
        var player = Create.Player(bunchId: bunch.Id);
        _playerRepository.Get(player.Id).Returns(player);
        
        var request = CreateRequest(playerId: player.Id);
        var result = await Sut.Execute(request);

        result.Success.Should().BeTrue();
        result.Data!.DisplayName.Should().Be(player.DisplayName);
        result.Data!.PlayerId.Should().Be(player.Id);
        result.Data!.AvatarUrl.Should().Be("");
        result.Data!.UserName.Should().BeNull();
        result.Data!.IsUser.Should().BeFalse();
        result.Data!.CanDelete.Should().BeFalse();
    }

    [Fact]
    public async Task PlayerDetails_WithUser_AvatarUrlIsSet()
    {
        var user = Create.User(email: "email@example.com");
        _userRepository.GetById(user.Id).Returns(user);
        var bunch = Create.Bunch();
        _bunchRepository.Get(bunch.Id).Returns(bunch);
        var player = Create.Player(bunchId: bunch.Id, userId: user.Id);
        _playerRepository.Get(player.Id).Returns(player);
        
        var request = CreateRequest(playerId: player.Id);
        var result = await Sut.Execute(request);

        const string expected = "https://gravatar.com/avatar/5658ffccee7f0ebfda2b226238b1eb6e?s=100&d=blank";
        result.Data!.AvatarUrl.Should().Be(expected);
        result.Data!.UserName.Should().Be(user.UserName);
        result.Data!.IsUser.Should().BeTrue();
    }

    [Fact]
    public async Task PlayerDetails_WithDeletePermissionAndPlayerHasNotPlayedGames_CanDeleteIsTrue()
    {
        var bunch = Create.Bunch();
        _bunchRepository.Get(bunch.Id).Returns(bunch);
        var player = Create.Player(bunchId: bunch.Id);
        _playerRepository.Get(player.Id).Returns(player);
        _cashgameRepository.GetByPlayer(player.Id).Returns([]);
        
        var request = CreateRequest(playerId: player.Id, canDeletePlayer: true);
        var result = await Sut.Execute(request);

        result.Data!.CanDelete.Should().BeTrue();
    }

    [Fact]
    public async Task PlayerDetails_DeletePermissionButPlayerHasPlayedGames_CanDeleteIsFalse()
    {
        var bunch = Create.Bunch();
        _bunchRepository.Get(bunch.Id).Returns(bunch);
        var player = Create.Player(bunchId: bunch.Id);
        _playerRepository.Get(player.Id).Returns(player);
        var cashgame = Create.Cashgame();
        _cashgameRepository.GetByPlayer(player.Id).Returns([cashgame]);
        
        var request = CreateRequest(playerId: player.Id, canDeletePlayer: true);
        var result = await Sut.Execute(request);

        result.Data!.CanDelete.Should().BeFalse();
    }

    private GetPlayer.Request CreateRequest(string? playerId = null, bool? canSeePlayer = null, bool? canDeletePlayer = null)
    {
        var auth = new AuthInTest(
            canSeePlayer: canSeePlayer ?? true,
            canDeletePlayer: canDeletePlayer ?? false);
        return new GetPlayer.Request(
            auth, 
            playerId ?? Create.String());
    }

    private GetPlayer Sut => new(
        _bunchRepository,
        _playerRepository,
        _cashgameRepository,
        _userRepository);
}