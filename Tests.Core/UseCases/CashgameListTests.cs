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

        Assert.AreEqual(TestData.SlugA, result.Data.Slug);
    }

    [Test]
    public async Task CashgameList_WithoutGames_HasEmptyListOfGames()
    {
        Deps.Cashgame.ClearList();

        var result = await Sut.Execute(CreateRequest());

        Assert.AreEqual(0, result.Data.Items.Count);
    }

    [Test]
    public async Task CashgameList_DefaultSort_FirstItemLocationIsSet()
    {
        var result = await Sut.Execute(CreateRequest());

        Assert.AreEqual(TestData.LocationNameB, result.Data.Items[0].LocationName);
    }

    [Test]
    public async Task CashgameList_DefaultSort_FirstItemUrlIsSet()
    {
        var result = await Sut.Execute(CreateRequest());

        Assert.AreEqual(2, result.Data.Items[0].CashgameId);
    }

    [Test]
    public async Task CashgameList_DefaultSort_FirstItemDurationIsSet()
    {
        var result = await Sut.Execute(CreateRequest());

        Assert.AreEqual(92, result.Data.Items[0].Duration.Minutes);
    }

    [Test]
    public async Task CashgameList_DefaultSort_FirstItemDateIsSet()
    {
        var result = await Sut.Execute(CreateRequest());

        var expected = new Date(2002, 2, 2);
        Assert.AreEqual(expected, result.Data.Items[0].Date);
    }

    [Test]
    public async Task CashgameList_DefaultSort_FirstItemPlayerCountIsSet()
    {
        var result = await Sut.Execute(CreateRequest());

        Assert.AreEqual(2, result.Data.Items[0].PlayerCount);
    }

    [Test]
    public async Task CashgameList_DefaultSort_FirstItemTurnoverIsSet()
    {
        var result = await Sut.Execute(CreateRequest());

        Assert.AreEqual(600, result.Data.Items[0].Turnover.Amount);
    }

    [Test]
    public async Task CashgameList_DefaultSort_FirstItemAverageBuyinIsSet()
    {
        var result = await Sut.Execute(CreateRequest());

        Assert.AreEqual(300, result.Data.Items[0].AverageBuyin.Amount);
    }

    private CashgameList.Request CreateRequest(CashgameList.SortOrder orderBy = CashgameList.SortOrder.Date, int? year = null)
    {
        return new CashgameList.Request(TestData.UserNameA, TestData.SlugA, orderBy, year);
    }

    private CashgameList Sut => new(
        Deps.Bunch,
        Deps.Cashgame,
        Deps.User,
        Deps.Player,
        Deps.Location);
}