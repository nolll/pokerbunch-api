using Core.Errors;
using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

public class DeleteCashgameTests : TestBase
{
    [Test]
    public async Task DeleteCashgame_GameHasResults_ReturnsError()
    {
        var request = new DeleteCashgame.Request(TestData.ManagerUser.UserName, TestData.CashgameIdA);
        var result = await Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Conflict));
    }

    [Test]
    public async Task DeleteCashgame_GameHasNoResults_DeletesGame()
    {
        Deps.Cashgame.SetupEmptyGame();

        var request = new DeleteCashgame.Request(TestData.ManagerUser.UserName, TestData.CashgameIdA);

        await Sut.Execute(request);

        Assert.That(Deps.Cashgame.Deleted, Is.EqualTo(TestData.CashgameIdA));
    }

    [Test]
    public async Task DeleteCashgame_GameHasNoResults_ReturnUrlIsSet()
    {
        Deps.Cashgame.SetupEmptyGame();

        var request = new DeleteCashgame.Request(TestData.ManagerUser.UserName, TestData.CashgameIdA);

        var result = await Sut.Execute(request);

        Assert.That(result.Data.Slug, Is.EqualTo(TestData.SlugA));
    }

    private DeleteCashgame Sut => new(
        Deps.Cashgame,
        Deps.Bunch,
        Deps.User,
        Deps.Player);
}