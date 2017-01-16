using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class CashgameDetailsTests : TestBase
    {
        [Test]
        public void RunningCashgame_CashgameNotRunning_ThrowsException()
        {
            //var request = new CashgameDetails2.Request(TestData.UserNameA, TestData.CashgameIdC);

            //Assert.Throws<CashgameNotRunningException>(() => Sut.Execute(request));
        }

        [Test]
        public void CashgameDetails_CashgameRunning_AllSimplePropertiesAreSet()
        {
            Deps.Cashgame.SetupRunningGame();

            var request = new CashgameDetails.Request(TestData.UserNameA, TestData.CashgameIdC);
            var result = Sut.Execute(request);

            Assert.AreEqual(TestData.PlayerIdA, result.PlayerId);
            Assert.AreEqual(TestData.LocationNameC, result.LocationName);
            Assert.AreEqual(100, result.DefaultBuyin);
            Assert.IsFalse(result.IsManager);
        }

        [Test]
        public void CashgameDetails_CashgameRunning_SlugIsSet()
        {
            Deps.Cashgame.SetupRunningGame();

            var request = new CashgameDetails.Request(TestData.UserNameA, TestData.CashgameIdC);
            var result = Sut.Execute(request);

            Assert.AreEqual("bunch-a", result.Slug);
        }

        [Test]
        public void CashgameDetails_CashgameRunning_PlayerItemsAreSet()
        {
            Deps.Cashgame.SetupRunningGame();

            var request = new CashgameDetails.Request(TestData.UserNameA, TestData.CashgameIdC);
            var result = Sut.Execute(request);

            Assert.AreEqual(2, result.PlayerItems.Count);
            Assert.AreEqual(1, result.PlayerItems[0].Checkpoints.Count);
            Assert.IsFalse(result.PlayerItems[0].HasCashedOut);
            Assert.AreEqual(TestData.PlayerA.DisplayName, result.PlayerItems[0].Name);
            Assert.AreEqual(TestData.PlayerA.Id, result.PlayerItems[0].PlayerId);
            Assert.AreEqual(3, result.PlayerItems[0].CashgameId);
            Assert.AreEqual(1, result.PlayerItems[0].PlayerId);
            Assert.AreEqual(1, result.PlayerItems[1].Checkpoints.Count);
            Assert.IsFalse(result.PlayerItems[1].HasCashedOut);
            Assert.AreEqual(TestData.PlayerB.DisplayName, result.PlayerItems[1].Name);
            Assert.AreEqual(TestData.PlayerB.Id, result.PlayerItems[1].PlayerId);
            Assert.AreEqual(3, result.PlayerItems[1].CashgameId);
            Assert.AreEqual(2, result.PlayerItems[1].PlayerId);
        }

        private CashgameDetails Sut => new CashgameDetails(
            Deps.Bunch,
            Deps.Cashgame,
            Deps.Player,
            Deps.User,
            Deps.Location);
    }
}