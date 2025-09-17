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

        var userBunch = Create.UserBunch(TestData.BunchIdA, TestData.SlugA, "", "", "", Role.None);
        var request = new DeletePlayer.Request(new AuthInTest(canDeletePlayer: true, userBunch: userBunch), playerIdThatHasNotPlayed);
        var result = await Sut.Execute(request);

        result.Success.Should().BeTrue();
        Deps.Player.Deleted.Should().Be(playerIdThatHasNotPlayed);
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