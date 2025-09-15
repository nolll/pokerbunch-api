using Core.Errors;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class DeleteCashgameTests : TestBase
{
    [Test]
    public async Task DeleteCashgame_GameHasResults_ReturnsError()
    {
        var request = new DeleteCashgame.Request(new AuthInTest(canDeleteCashgame: true), TestData.CashgameIdA);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Conflict);
    }

    [Test]
    public async Task DeleteCashgame_GameHasNoResults_DeletesGame()
    {
        Deps.Cashgame.SetupEmptyGame();

        var request = new DeleteCashgame.Request(new AuthInTest(canDeleteCashgame: true), TestData.CashgameIdA);

        await Sut.Execute(request);

        Deps.Cashgame.Deleted.Should().Be(TestData.CashgameIdA);
    }

    [Test]
    public async Task DeleteCashgame_GameHasNoResults_ReturnUrlIsSet()
    {
        Deps.Cashgame.SetupEmptyGame();

        var request = new DeleteCashgame.Request(new AuthInTest(canDeleteCashgame: true), TestData.CashgameIdA);

        var result = await Sut.Execute(request);

        result.Data!.Slug.Should().Be(TestData.SlugA);
    }

    private DeleteCashgame Sut => new(
        Deps.Cashgame,
        Deps.Bunch);
}