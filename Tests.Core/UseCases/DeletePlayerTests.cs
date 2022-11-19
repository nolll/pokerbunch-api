using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

public class DeletePlayerTests : TestBase
{
    [Test]
    public async Task DeletePlayer_PlayerHasntPlayed_PlayerDeletedAndReturnUrlIsPlayerIndex()
    {
        const int playerIdThatHasNotPlayed = 3;

        var request = new DeletePlayer.Request(TestData.ManagerUser.UserName, playerIdThatHasNotPlayed);
        var result = await Sut.Execute(request);

        Assert.AreEqual(TestData.SlugA, result.Data.Slug);
        Assert.AreEqual(playerIdThatHasNotPlayed, result.Data.PlayerId);
        Assert.AreEqual(playerIdThatHasNotPlayed, Deps.Player.Deleted);
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
        Deps.Cashgame,
        Deps.User,
        Deps.Bunch);
}