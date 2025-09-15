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

        result.Data!.Slug.Should().Be("bunch-a");
        result.Data!.Players.Count.Should().Be(4);
        result.Data!.Players[0].Id.Should().Be("1");
        result.Data!.Players[0].Name.Should().Be(TestData.PlayerNameA);
        result.Data!.CanAddPlayer.Should().BeFalse();
    }

    [Test]
    public async Task Execute_PlayersAreSortedAlphabetically()
    {
        var request = new GetPlayerList.Request(new AuthInTest(canListPlayers: true), TestData.SlugA);

        var result = await Sut.Execute(request);

        result.Data!.Players[0].Name.Should().Be(TestData.PlayerNameA);
        result.Data!.Players[1].Name.Should().Be(TestData.PlayerNameB);
    }

    [Test]
    public async Task Execute_PlayerIsManager_CanAddPlayerIsTrue()
    {
        var request = new GetPlayerList.Request(new AuthInTest(canListPlayers: true, canAddPlayer: true), TestData.SlugA);

        var result = await Sut.Execute(request);

        result.Data!.CanAddPlayer.Should().BeTrue();
    }

    private GetPlayerList Sut => new(
        Deps.Bunch,
        Deps.Player);
}