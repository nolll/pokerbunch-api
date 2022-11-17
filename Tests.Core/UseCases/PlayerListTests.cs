using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

class PlayerListTests : TestBase
{
    [Test]
    public void Execute_WithSlug_SlugAndPlayersAreSet()
    {
        var request = new GetPlayerList.Request(TestData.UserNameA, TestData.SlugA);

        var result = Sut.Execute(request);

        Assert.AreEqual("bunch-a", result.Data.Slug);
        Assert.AreEqual(4, result.Data.Players.Count);
        Assert.AreEqual(1, result.Data.Players[0].Id);
        Assert.AreEqual(TestData.PlayerNameA, result.Data.Players[0].Name);
        Assert.IsFalse(result.Data.CanAddPlayer);
    }

    [Test]
    public void Execute_PlayersAreSortedAlphabetically()
    {
        var request = new GetPlayerList.Request(TestData.UserNameA, TestData.SlugA);

        var result = Sut.Execute(request);

        Assert.AreEqual(TestData.PlayerNameA, result.Data.Players[0].Name);
        Assert.AreEqual(TestData.PlayerNameB, result.Data.Players[1].Name);
    }

    [Test]
    public void Execute_PlayerIsManager_CanAddPlayerIsTrue()
    {
        var request = new GetPlayerList.Request(TestData.UserNameC, TestData.SlugA);

        var result = Sut.Execute(request);

        Assert.IsTrue(result.Data.CanAddPlayer);
    }

    private GetPlayerList Sut => new(
        Deps.Bunch,
        Deps.User,
        Deps.Player);
}