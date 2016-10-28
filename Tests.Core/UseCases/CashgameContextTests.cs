using System;
using System.Linq;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    class CashgameContextTests : TestBase
    {
        [Test]
        public void Execute_NoRunningGame_GameIsRunningIsFalse()
        {
            const string slug = "a";
            const int year = 1;
            var request = new CashgameContext.Request(TestData.UserNameA, slug, DateTime.UtcNow, CashgameContext.CashgamePage.Unknown, year);

            var result = Sut.Execute(request);

            Assert.IsFalse(result.GameIsRunning);
        }

        [Test]
        public void Execute_BunchContextIsSet()
        {
            const string slug = "a";
            var cashgameContextRequest = new CashgameContext.Request(TestData.UserNameA, slug, DateTime.UtcNow);

            var result = Sut.Execute(cashgameContextRequest);

            Assert.IsInstanceOf<BunchContext.Result>(result.BunchContext);
        }

        [Test]
        public void Execute_WithYear_SelectedYearIsSet()
        {
            const string slug = "a";
            const int year = 1;
            var request = new CashgameContext.Request(TestData.UserNameA, slug, DateTime.UtcNow, CashgameContext.CashgamePage.Unknown, year);

            var result = Sut.Execute(request);

            Assert.AreEqual(year, result.SelectedYear);
        }

        [Test]
        public void Execute_WithoutYear_SelectedYearIsNull()
        {
            const string slug = "a";
            var request = new CashgameContext.Request(TestData.UserNameA, slug, DateTime.UtcNow);

            var result = Sut.Execute(request);

            Assert.IsNull(result.SelectedYear);
        }

        [Test]
        public void Execute_WithoutRunningGame_GameIsRunningGameIsFalse()
        {
            const string slug = "a";
            const int year = 1;
            var request = new CashgameContext.Request(TestData.UserNameA, slug, DateTime.UtcNow, CashgameContext.CashgamePage.Unknown, year);

            var result = Sut.Execute(request);

            Assert.IsFalse(result.GameIsRunning);
        }

        [Test]
        public void Execute_WithRunningGame_GameIsRunningIsTrue()
        {
            Repos.Cashgame.SetupRunningGame();

            const string slug = "a";
            var request = new CashgameContext.Request(TestData.UserNameA, slug, DateTime.UtcNow);

            var result = Sut.Execute(request);

            Assert.IsTrue(result.GameIsRunning);
        }

        [TestCase(CashgameContext.CashgamePage.Matrix)]
        [TestCase(CashgameContext.CashgamePage.Toplist)]
        [TestCase(CashgameContext.CashgamePage.Chart)]
        [TestCase(CashgameContext.CashgamePage.List)]
        [TestCase(CashgameContext.CashgamePage.Facts)]
        public void Execute_SelectedPage_SelectedPageIsCorrect(CashgameContext.CashgamePage selectedPage)
        {
            const string slug = "a";
            const int year = 1;
            var request = new CashgameContext.Request(TestData.UserNameA, slug, DateTime.UtcNow, selectedPage, year);

            var result = Sut.Execute(request);

            Assert.AreEqual(selectedPage, result.SelectedPage);
            Assert.AreEqual(2002, result.Years.Last());
        }

        [Test]
        public void Execute_SelectedPage_YearsAreCorrect()
        {
            const string slug = "a";
            const int year = 1;
            var request = new CashgameContext.Request(TestData.UserNameA, slug, DateTime.UtcNow, CashgameContext.CashgamePage.Matrix, year);

            var result = Sut.Execute(request);

            Assert.AreEqual(2001, result.Years.First());
            Assert.AreEqual(2002, result.Years.Last());
        }

        private CashgameContext Sut => new CashgameContext(
            Repos.User,
            Repos.Bunch,
            Services.CashgameService);
    }
}