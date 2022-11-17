﻿using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

class PlayerDetailsTests : TestBase
{
    [Test]
    public void PlayerDetails_DisplayNameIsSet()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.AreEqual(TestData.PlayerNameA, result.Data.DisplayName);
    }

    [Test]
    public void PlayerDetails_DeleteUrlIsSet()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.AreEqual(1, result.Data.PlayerId);
    }

    [Test]
    public void PlayerDetails_WithoutUser_AvatarUrlIsEmpty()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdD));

        Assert.AreEqual("", result.Data.AvatarUrl);
    }

    [Test]
    public void PlayerDetails_WithUser_AvatarUrlIsSet()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        const string expected = "https://gravatar.com/avatar/0796c9df772de3f82c0c89377330471b?s=100&d=blank";
        Assert.AreEqual(expected, result.Data.AvatarUrl);
    }

    [Test]
    public void PlayerDetails_WithoutUser_UserUrlIsEmpty()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdD));

        Assert.AreEqual(string.Empty, result.Data.UserName);
    }

    [Test]
    public void PlayerDetails_WithUser_UserUrlIsSet()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.AreEqual("user-name-a", result.Data.UserName);
    }

    [Test]
    public void PlayerDetails_WithoutUser_IsUserIsFalse()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdD));

        Assert.IsFalse(result.Data.IsUser);
    }

    [Test]
    public void PlayerDetails_WithUser_IsUserIsTrue()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.IsTrue(result.Data.IsUser);
    }

    [Test]
    public void PlayerDetails_WithNormalUser_CanDeleteIsFalse()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.IsFalse(result.Data.CanDelete);
    }

    [Test]
    public void PlayerDetails_WithManagerAndPlayerHasNotPlayedGames_CanDeleteIsTrue()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameC, TestData.PlayerIdD));

        Assert.IsTrue(result.Data.CanDelete);
    }

    [Test]
    public void PlayerDetails_WithManagerAndPlayerHasPlayedGames_CanDeleteIsFalse()
    {
        var result = Sut.Execute(CreateRequest(TestData.UserNameA, TestData.PlayerIdA));

        Assert.IsFalse(result.Data.CanDelete);
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