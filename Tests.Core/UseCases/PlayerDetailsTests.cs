using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

class PlayerDetailsTests : TestBase
{
    [Test]
    public async Task PlayerDetails_DisplayNameIsSet()
    {
        var result = await Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.That(result.Data.DisplayName, Is.EqualTo(TestData.PlayerNameA));
    }

    [Test]
    public async Task PlayerDetails_DeleteUrlIsSet()
    {
        var result = await Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.That(result.Data.PlayerId, Is.EqualTo(1));
    }

    [Test]
    public async Task PlayerDetails_WithoutUser_AvatarUrlIsEmpty()
    {
        var result = await Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdD));

        Assert.That(result.Data.AvatarUrl, Is.EqualTo(""));
    }

    [Test]
    public async Task PlayerDetails_WithUser_AvatarUrlIsSet()
    {
        var result = await Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        const string expected = "https://gravatar.com/avatar/0796c9df772de3f82c0c89377330471b?s=100&d=blank";
        Assert.That(result.Data.AvatarUrl, Is.EqualTo(expected));
    }

    [Test]
    public async Task PlayerDetails_WithoutUser_UserUrlIsEmpty()
    {
        var result = await Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdD));

        Assert.That(result.Data.UserName, Is.EqualTo(string.Empty));
    }

    [Test]
    public async Task PlayerDetails_WithUser_UserUrlIsSet()
    {
        var result = await Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.That(result.Data.UserName, Is.EqualTo("user-name-a"));
    }

    [Test]
    public async Task PlayerDetails_WithoutUser_IsUserIsFalse()
    {
        var result = await Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdD));

        Assert.That(result.Data.IsUser, Is.False);
    }

    [Test]
    public async Task PlayerDetails_WithUser_IsUserIsTrue()
    {
        var result = await Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.That(result.Data.IsUser, Is.True);
    }

    [Test]
    public async Task PlayerDetails_WithNormalUser_CanDeleteIsFalse()
    {
        var result = await Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.That(result.Data.CanDelete, Is.False);
    }

    [Test]
    public async Task PlayerDetails_WithManagerAndPlayerHasNotPlayedGames_CanDeleteIsTrue()
    {
        var result = await Sut.Execute(CreateRequest(TestData.UserNameC, TestData.PlayerIdD));

        Assert.That(result.Data.CanDelete, Is.True);
    }

    [Test]
    public async Task PlayerDetails_WithManagerAndPlayerHasPlayedGames_CanDeleteIsFalse()
    {
        var result = await Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.That(result.Data.CanDelete, Is.False);
    }

    private static GetPlayer.Request CreateRequest(string userName, int playerId)
    {
        return new GetPlayer.Request(userName, playerId);
    }

    private GetPlayer Sut => new(
        Deps.Bunch,
        Deps.Player,
        Deps.Cashgame,
        Deps.User);
}