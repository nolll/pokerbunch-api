using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

class PlayerListTests : TestBase
{
    [Test]
    public async Task Execute_WithSlug_SlugAndPlayersAreSet()
    {
        var request = new GetPlayerList.Request(new AuthInTest(canListPlayers: true), TestData.SlugA);

        var result = await Sut.Execute(request);

        Assert.That(result.Data?.Slug, Is.EqualTo("bunch-a"));
        Assert.That(result.Data?.Players.Count, Is.EqualTo(4));
        Assert.That(result.Data?.Players[0].Id, Is.EqualTo("1"));
        Assert.That(result.Data?.Players[0].Name, Is.EqualTo(TestData.PlayerNameA));
        Assert.That(result.Data?.CanAddPlayer, Is.False);
    }

    [Test]
    public async Task Execute_PlayersAreSortedAlphabetically()
    {
        var request = new GetPlayerList.Request(new AuthInTest(canListPlayers: true), TestData.SlugA);

        var result = await Sut.Execute(request);

        Assert.That(result.Data?.Players[0].Name, Is.EqualTo(TestData.PlayerNameA));
        Assert.That(result.Data?.Players[1].Name, Is.EqualTo(TestData.PlayerNameB));
    }

    [Test]
    public async Task Execute_PlayerIsManager_CanAddPlayerIsTrue()
    {
        var request = new GetPlayerList.Request(new AuthInTest(canListPlayers: true, canAddPlayer: true), TestData.SlugA);

        var result = await Sut.Execute(request);

        Assert.That(result.Data?.CanAddPlayer, Is.True);
    }

    private GetPlayerList Sut => new(
        Deps.Bunch,
        Deps.Player);
}