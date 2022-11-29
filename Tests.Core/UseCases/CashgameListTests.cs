using Core.Entities;
using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

class CashgameListTests : TestBase
{
    private const int Year = 2001;

    [Test]
    public async Task CashgameList_SlugIsSet()
    {
        var result = await Sut.Execute(CreateRequest());

        Assert.That(result.Data.Slug, Is.EqualTo(TestData.SlugA));
    }

    [Test]
    public async Task CashgameList_WithoutGames_HasEmptyListOfGames()
    {
        Deps.Cashgame.ClearList();

        var result = await Sut.Execute(CreateRequest());

        Assert.That(result.Data.Items.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task CashgameList_DefaultSort_FirstItemLocationIsSet()
    {
        var result = await Sut.Execute(CreateRequest());

        Assert.That(result.Data.Items[0].LocationName, Is.EqualTo(TestData.LocationNameB));
    }

    [Test]
    public async Task CashgameList_DefaultSort_FirstItemUrlIsSet()
    {
        var result = await Sut.Execute(CreateRequest());

        Assert.That(result.Data.Items[0].CashgameId, Is.EqualTo("2"));
    }
    
    private CashgameList.Request CreateRequest(int? year = null)
    {
        return new CashgameList.Request(TestData.UserNameA, TestData.SlugA, year);
    }

    private CashgameList Sut => new(
        Deps.Bunch,
        Deps.Cashgame,
        Deps.User,
        Deps.Player,
        Deps.Location);
}