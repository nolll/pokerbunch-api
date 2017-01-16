using System;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    class CashgameDetailsOldTests : TestBase
    {
        [Test]
        public void CashgameDetailsOld_AllBaseValuesAreSet()
        {
            var request = new CashgameDetailsOld.Request(TestData.UserNameA, TestData.CashgameIdA);

            var result = Sut.Execute(request);

            Assert.AreEqual(TestData.DateStringA, result.Date.IsoString);
            Assert.AreEqual(TestData.LocationNameA, result.LocationName);
            Assert.AreEqual(62, result.Duration.Minutes);
            Assert.AreEqual(DateTime.Parse("2001-01-01 11:00:00"), result.StartTime);
            Assert.AreEqual(DateTime.Parse("2001-01-01 12:02:00"), result.EndTime);
            Assert.IsFalse(result.CanEdit);
            Assert.AreEqual(1, result.CashgameId);
            Assert.AreEqual(2, result.PlayerItems.Count);
        }

        [Test]
        public void CashgameDetailsOld_WithResultsAndPlayers_PlayerResultItemsCountAndOrderIsCorrect()
        {
            var request = new CashgameDetailsOld.Request(TestData.UserNameA, TestData.CashgameIdA);

            var result = Sut.Execute(request);

            Assert.AreEqual(2, result.PlayerItems.Count);
            Assert.AreEqual(150, result.PlayerItems[0].Winnings.Amount);
            Assert.AreEqual(-150, result.PlayerItems[1].Winnings.Amount);
        }

        [Test]
        public void CashgameDetailsOld_AllResultItemPropertiesAreSet()
        {
            var request = new CashgameDetailsOld.Request(TestData.UserNameA, TestData.CashgameIdA);

            var result = Sut.Execute(request);

            Assert.AreEqual(TestData.PlayerNameB, result.PlayerItems[0].Name);
            Assert.AreEqual(1, result.PlayerItems[0].CashgameId);
            Assert.AreEqual(2, result.PlayerItems[0].PlayerId);
            Assert.AreEqual(200, result.PlayerItems[0].Buyin.Amount);
            Assert.AreEqual(350, result.PlayerItems[0].Cashout.Amount);
            Assert.AreEqual(150, result.PlayerItems[0].Winnings.Amount);
            Assert.AreEqual(148, result.PlayerItems[0].WinRate.Amount);
        }

        [Test]
        public void CashgameDetailsOld_WithManager_CanEditIsTrue()
        {
            var request = new CashgameDetailsOld.Request(TestData.UserNameC, TestData.CashgameIdA);

            var result = Sut.Execute(request);

            Assert.IsTrue(result.CanEdit);
        }

        private CashgameDetailsOld Sut => new CashgameDetailsOld(
            Deps.Bunch,
            Deps.Cashgame,
            Deps.User,
            Deps.Player,
            Deps.Location);
    }
}