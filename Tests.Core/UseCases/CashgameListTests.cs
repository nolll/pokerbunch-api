using Core.Entities;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class CashgameListTests : TestBase
{
    [Test]
    public async Task CashgameList_SlugIsSet()
    {
        var result = await Sut.Execute(CreateRequest());

        result.Data!.Slug.Should().Be(TestData.SlugA);
    }

    [Test]
    public async Task CashgameList_WithoutGames_HasEmptyListOfGames()
    {
        Deps.Cashgame.ClearList();

        var result = await Sut.Execute(CreateRequest());

        result.Data!.Items.Count.Should().Be(0);
    }

    [Test]
    public async Task CashgameList_DefaultSort_FirstItemLocationIsSet()
    {
        var result = await Sut.Execute(CreateRequest());

        result.Data!.Items[0].LocationName.Should().Be(TestData.LocationNameB);
    }

    [Test]
    public async Task CashgameList_DefaultSort_FirstItemUrlIsSet()
    {
        var result = await Sut.Execute(CreateRequest());

        result.Data!.Items[0].CashgameId.Should().Be("2");
    }
    
    private CashgameList.Request CreateRequest(int? year = null)
    {
        var currentBunch = new CurrentBunch(TestData.BunchA.Id, TestData.BunchA.Slug, "", "", "", Role.None);
        return new CashgameList.Request(new AuthInTest(canListCashgames: true, currentBunch: currentBunch), TestData.SlugA, year);
    }

    private CashgameList Sut => new(
        Deps.Cashgame,
        Deps.Player,
        Deps.Location);
}