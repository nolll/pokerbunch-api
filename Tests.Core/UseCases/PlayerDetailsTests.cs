using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

class PlayerDetailsTests : TestBase
{
    [Test]
    public void PlayerDetails_DisplayNameIsSet()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.AreEqual(TestData.PlayerNameA, result.DisplayName);
    }

    [Test]
    public void PlayerDetails_DeleteUrlIsSet()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.AreEqual(1, result.PlayerId);
    }

    [Test]
    public void PlayerDetails_WithoutUser_AvatarUrlIsEmpty()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdD));

        Assert.AreEqual("", result.AvatarUrl);
    }

    [Test]
    public void PlayerDetails_WithUser_AvatarUrlIsSet()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        const string expected = "https://www.gravatar.com/avatar/0796c9df772de3f82c0c89377330471b?s=100";
        Assert.AreEqual(expected, result.AvatarUrl);
    }

    [Test]
    public void PlayerDetails_WithoutUser_UserUrlIsEmpty()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdD));

        Assert.AreEqual(string.Empty, result.UserName);
    }

    [Test]
    public void PlayerDetails_WithUser_UserUrlIsSet()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.AreEqual("user-name-a", result.UserName);
    }

    [Test]
    public void PlayerDetails_WithoutUser_IsUserIsFalse()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdD));

        Assert.IsFalse(result.IsUser);
    }

    [Test]
    public void PlayerDetails_WithUser_IsUserIsTrue()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.IsTrue(result.IsUser);
    }

    [Test]
    public void PlayerDetails_WithNormalUser_CanDeleteIsFalse()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.IsFalse(result.CanDelete);
    }

    [Test]
    public void PlayerDetails_WithManagerAndPlayerHasNotPlayedGames_CanDeleteIsTrue()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameC, TestData.PlayerIdD));

        Assert.IsTrue(result.CanDelete);
    }

    [Test]
    public void PlayerDetails_WithManagerAndPlayerHasPlayedGames_CanDeleteIsFalse()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.IsFalse(result.CanDelete);
    }

    private static GetPlayer.Request CreateRequest(string userName, int playerId)
    {
        return new GetPlayer.Request(userName, playerId);
    }

    private GetPlayer Sut => new GetPlayer(
        Deps.Bunch,
        Deps.Player,
        Deps.Cashgame,
        Deps.User);
}