using Core.Entities;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class DeletePlayerTests : TestBase
{
    [Test]
    public async Task DeletePlayer_PlayerHasntPlayed_PlayerDeletedAndReturnUrlIsPlayerIndex()
    {
        const string playerIdThatHasNotPlayed = "3";

        var currentBunch = new CurrentBunch(TestData.BunchIdA, TestData.SlugA, "", "", "", Role.None);
        var request = new DeletePlayer.Request(new AuthInTest(canDeletePlayer: true, currentBunch: currentBunch), playerIdThatHasNotPlayed);
        var result = await Sut.Execute(request);

        Assert.That(result.Data?.Slug, Is.EqualTo(TestData.SlugA));
        Assert.That(result.Data?.PlayerId, Is.EqualTo(playerIdThatHasNotPlayed));
        Assert.That(Deps.Player.Deleted, Is.EqualTo(playerIdThatHasNotPlayed));
    }

    // todo: This should throw an exception. Fix test during test rewrite
    //[Test]
    //public void DeletePlayer_PlayerHasPlayed_ReturnUrlIsPlayerDetails()
    //{
    //    var request = new DeletePlayer.Request(TestData.ManagerUser.UserName, TestData.PlayerIdA);
    //    var result = Sut.Execute(request);
    //}

    private DeletePlayer Sut => new(
        Deps.Player,
        Deps.Cashgame);
}