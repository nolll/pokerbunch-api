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
        public void CashgameList_WithoutYear_YearIsNull()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.IsFalse(result.ShowYear);
            Assert.IsNull(result.Year);
        }

        [Test]
        public void CashgameList_WithoutGames_HasEmptyListOfGames()
        {
            Repos.Cashgame.ClearList();

            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(0, result.List.Count);
        }

        [Test]
        public void CashgameList_WithYear_YearIsSet()
        {
            var result = Sut.Execute(CreateRequest(year: Year));

            Assert.IsTrue(result.ShowYear);
            Assert.AreEqual(Year, result.Year);
        }

        [Test]
        public void CashgameList_DefaultSort_FirstItemLocationIsSet()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(TestData.LocationNameB, result.List[0].Location);
        }

        [Test]
        public void CashgameList_DefaultSort_FirstItemUrlIsSet()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(2, result.List[0].CashgameId);
        }

        [Test]
        public void CashgameList_DefaultSort_FirstItemDurationIsSet()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(92, result.List[0].Duration.Minutes);
        }

        [Test]
        public void CashgameList_DefaultSort_FirstItemDateIsSet()
        {
            var result = Sut.Execute(CreateRequest());

            var expected = new Date(2002, 2, 2);
            Assert.AreEqual(expected, result.List[0].Date);
        }

        [Test]
        public void CashgameList_DefaultSort_FirstItemPlayerCountIsSet()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(2, result.List[0].PlayerCount);
        }

        [Test]
        public void CashgameList_DefaultSort_FirstItemTurnoverIsSet()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(600, result.List[0].Turnover.Amount);
        }

        [Test]
        public void CashgameList_DefaultSort_FirstItemAverageBuyinIsSet()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(300, result.List[0].AverageBuyin.Amount);
        }

        [Test]
        public void TopList_SortByWinnings_HighestWinningsIsFirst()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(CashgameList.SortOrder.Date, result.SortOrder);
            Assert.AreEqual(new Date(2002, 2, 2), result.List[0].Date);
            Assert.AreEqual(new Date(2001, 1, 1), result.List[1].Date);
        }

        [Test]
        public void TopList_SortByPlayerCount_HighestPlayerCountIsFirst()
        {
            var result = Sut.Execute(CreateRequest(CashgameList.SortOrder.PlayerCount));

            Assert.AreEqual(CashgameList.SortOrder.PlayerCount, result.SortOrder);
            Assert.AreEqual(2, result.List[0].PlayerCount);
            Assert.AreEqual(2, result.List[1].PlayerCount);
        }

        [Test]
        public void TopList_SortByLocation_HighestLocationIsFirst()
        {
            var result = Sut.Execute(CreateRequest(CashgameList.SortOrder.Location));

            Assert.AreEqual(CashgameList.SortOrder.Location, result.SortOrder);
            Assert.AreEqual(TestData.LocationNameB, result.List[0].Location);
            Assert.AreEqual(TestData.LocationNameA, result.List[1].Location);
        }

        [Test]
        public void TopList_SortByDuration_HighestDurationIsFirst()
        {
            var result = Sut.Execute(CreateRequest(CashgameList.SortOrder.Duration));

            Assert.AreEqual(CashgameList.SortOrder.Duration, result.SortOrder);
            Assert.AreEqual(92, result.List[0].Duration.Minutes);
            Assert.AreEqual(62, result.List[1].Duration.Minutes);
        }

        [Test]
        public void TopList_SortByTurnover_HighestTurnoverIsFirst()
        {
            var result = Sut.Execute(CreateRequest(CashgameList.SortOrder.Turnover));

            Assert.AreEqual(CashgameList.SortOrder.Turnover, result.SortOrder);
            Assert.AreEqual(600, result.List[0].Turnover.Amount);
            Assert.AreEqual(400, result.List[1].Turnover.Amount);
        }

        [Test]
        public void TopList_SortByAverageBuyin_HighestAverageBuyinIsFirst()
        {
            var result = Sut.Execute(CreateRequest(CashgameList.SortOrder.AverageBuyin));

            Assert.AreEqual(CashgameList.SortOrder.AverageBuyin, result.SortOrder);
            Assert.AreEqual(300, result.List[0].AverageBuyin.Amount);
            Assert.AreEqual(200, result.List[1].AverageBuyin.Amount);
        }

        private CashgameList.Request CreateRequest(CashgameList.SortOrder orderBy = CashgameList.SortOrder.Date, int? year = null)
        {
            return new CashgameList.Request(TestData.UserNameA, TestData.SlugA, orderBy, year);
        }

        private CashgameList Sut
        {
            get
            {
                return new CashgameList(
                    Services.BunchService,
                    Services.CashgameService,
                    Services.UserService,
                    Services.PlayerService,
                    Services.LocationService);
            }
        }
    }
}
