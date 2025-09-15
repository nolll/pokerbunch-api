using Core.Services;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class PlayerDetailsTests : TestBase
{
    [Test]
    public async Task PlayerDetails_DisplayNameIsSet()
    {
        var result = await Sut.Execute(CreateRequest(new AuthInTest(canSeePlayer: true), TestData.PlayerIdA));

        result.Data!.DisplayName.Should().Be(TestData.PlayerNameA);
    }

    [Test]
    public async Task PlayerDetails_DeleteUrlIsSet()
    {
        var result = await Sut.Execute(CreateRequest(new AuthInTest(canSeePlayer: true), TestData.PlayerIdA));

        result.Data!.PlayerId.Should().Be("1");
    }

    [Test]
    public async Task PlayerDetails_WithoutUser_AvatarUrlIsEmpty()
    {
        var result = await Sut.Execute(CreateRequest(new AuthInTest(canSeePlayer: true), TestData.PlayerIdD));

        result.Data!.AvatarUrl.Should().Be("");
    }

    [Test]
    public async Task PlayerDetails_WithUser_AvatarUrlIsSet()
    {
        var result = await Sut.Execute(CreateRequest(new AuthInTest(canSeePlayer: true), TestData.PlayerIdA));

        const string expected = "https://gravatar.com/avatar/0796c9df772de3f82c0c89377330471b?s=100&d=blank";
        result.Data!.AvatarUrl.Should().Be(expected);
    }

    [Test]
    public async Task PlayerDetails_WithoutUser_UserUrlIsNull()
    {
        var result = await Sut.Execute(CreateRequest(new AuthInTest(canSeePlayer: true), TestData.PlayerIdD));

        result.Data!.UserName.Should().BeNull();
    }

    [Test]
    public async Task PlayerDetails_WithUser_UserUrlIsSet()
    {
        var result = await Sut.Execute(CreateRequest(new AuthInTest(canSeePlayer: true), TestData.PlayerIdA));

        result.Data!.UserName.Should().Be("user-name-a");
    }

    [Test]
    public async Task PlayerDetails_WithoutUser_IsUserIsFalse()
    {
        var result = await Sut.Execute(CreateRequest(new AuthInTest(canSeePlayer: true), TestData.PlayerIdD));

        result.Data!.IsUser.Should().BeFalse();
    }

    [Test]
    public async Task PlayerDetails_WithUser_IsUserIsTrue()
    {
        var result = await Sut.Execute(CreateRequest(new AuthInTest(canSeePlayer: true), TestData.PlayerIdA));

        result.Data!.IsUser.Should().BeTrue();
    }

    [Test]
    public async Task PlayerDetails_WithNormalUser_CanDeleteIsFalse()
    {
        var result = await Sut.Execute(CreateRequest(new AuthInTest(canSeePlayer: true, canDeletePlayer: false), TestData.PlayerIdA));

        result.Data!.CanDelete.Should().BeFalse();
    }

    [Test]
    public async Task PlayerDetails_WithManagerAndPlayerHasNotPlayedGames_CanDeleteIsTrue()
    {
        var result = await Sut.Execute(CreateRequest(new AuthInTest(canSeePlayer: true, canDeletePlayer: true), TestData.PlayerIdD));

        result.Data!.CanDelete.Should().BeTrue();
    }

    [Test]
    public async Task PlayerDetails_WithManagerAndPlayerHasPlayedGames_CanDeleteIsFalse()
    {
        var result = await Sut.Execute(CreateRequest(new AuthInTest(canSeePlayer: true, canDeletePlayer: true), TestData.PlayerIdA));

        result.Data!.CanDelete.Should().BeFalse();
    }

    private static GetPlayer.Request CreateRequest(IAuth auth, string playerId)
    {
        return new GetPlayer.Request(auth, playerId);
    }

    private GetPlayer Sut => new(
        Deps.Bunch,
        Deps.Player,
        Deps.Cashgame,
        Deps.User);
}