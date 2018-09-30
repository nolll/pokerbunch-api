using Core.Entities;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    class CashgameListTests : TestBase
    {
        private const int Year = 2001;

        [Test]
        public void CashgameList_SlugIsSet()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(TestData.SlugA, result.Slug);
        }

        [Test]
        public void CashgameList_WithoutGames_HasEmptyListOfGames()
        {
            Deps.Cashgame.ClearList();

            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(0, result.Items.Count);
        }

        [Test]
        public void CashgameList_DefaultSort_FirstItemLocationIsSet()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(TestData.LocationNameB, result.Items[0].LocationName);
        }

        [Test]
        public void CashgameList_DefaultSort_FirstItemUrlIsSet()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(2, result.Items[0].CashgameId);
        }

        [Test]
        public void CashgameList_DefaultSort_FirstItemDurationIsSet()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(92, result.Items[0].Duration.Minutes);
        }

        [Test]
        public void CashgameList_DefaultSort_FirstItemDateIsSet()
        {
            var result = Sut.Execute(CreateRequest());

            var expected = new Date(2002, 2, 2);
            Assert.AreEqual(expected, result.Items[0].Date);
        }

        [Test]
        public void CashgameList_DefaultSort_FirstItemPlayerCountIsSet()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(2, result.Items[0].PlayerCount);
        }

        [Test]
        public void CashgameList_DefaultSort_FirstItemTurnoverIsSet()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(600, result.Items[0].Turnover.Amount);
        }

        [Test]
        public void CashgameList_DefaultSort_FirstItemAverageBuyinIsSet()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(300, result.Items[0].AverageBuyin.Amount);
        }

        private CashgameList.Request CreateRequest(CashgameList.SortOrder orderBy = CashgameList.SortOrder.Date, int? year = null)
        {
            return new CashgameList.Request(TestData.UserNameA, TestData.SlugA, orderBy, year);
        }

        private CashgameList Sut => new CashgameList(
            Deps.Bunch,
            Deps.Cashgame,
            Deps.User,
            Deps.Player,
            Deps.Location);
    }
}
