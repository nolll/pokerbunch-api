using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class RunningCashgameTests : TestBase
    {
        [Test]
        public void RunningCashgame_CashgameNotRunning_ThrowsException()
        {
            var request = new RunningCashgame.Request(TestData.UserNameA, TestData.SlugA);

            Assert.Throws<CashgameNotRunningException>(() => Sut.Execute(request));
        }

        [Test]
        public void RunningCashgame_CashgameRunning_AllSimplePropertiesAreSet()
        {
            Repos.Cashgame.SetupRunningGame();

            var request = new RunningCashgame.Request(TestData.UserNameA, TestData.SlugA);
            var result = Sut.Execute(request);

            Assert.AreEqual(TestData.PlayerIdA, result.PlayerId);
            Assert.AreEqual(TestData.LocationNameC, result.LocationName);
            Assert.AreEqual(100, result.DefaultBuyin);
            Assert.IsFalse(result.IsManager);
        }

        [Test]
        public void RunningCashgame_CashgameRunning_SlugIsSet()
        {
            Repos.Cashgame.SetupRunningGame();

            var request = new RunningCashgame.Request(TestData.UserNameA, TestData.SlugA);
            var result = Sut.Execute(request);

            Assert.AreEqual("bunch-a", result.Slug);
        }

        [Test]
        public void RunningCashgame_CashgameRunning_PlayerItemsAreSet()
        {
            Repos.Cashgame.SetupRunningGame();

            var request = new RunningCashgame.Request(TestData.UserNameA, TestData.SlugA);
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

        [Test]
        public void RunningCashgame_CashgameRunning_BunchPlayerItemsAreSet()
        {
            Repos.Cashgame.SetupRunningGame();

            var request = new RunningCashgame.Request(TestData.UserNameA, TestData.SlugA);
            var result = Sut.Execute(request);

            Assert.AreEqual(4, result.BunchPlayerItems.Count);
            Assert.AreEqual(TestData.PlayerA.DisplayName, result.BunchPlayerItems[0].Name);
            Assert.AreEqual(TestData.PlayerA.Id, result.BunchPlayerItems[0].PlayerId);
            Assert.AreEqual(TestData.PlayerB.DisplayName, result.BunchPlayerItems[1].Name);
            Assert.AreEqual(TestData.PlayerB.Id, result.BunchPlayerItems[1].PlayerId);
            Assert.AreEqual(TestData.PlayerC.DisplayName, result.BunchPlayerItems[2].Name);
            Assert.AreEqual(TestData.PlayerC.Id, result.BunchPlayerItems[2].PlayerId);
            Assert.AreEqual(TestData.PlayerD.DisplayName, result.BunchPlayerItems[3].Name);
            Assert.AreEqual(TestData.PlayerD.Id, result.BunchPlayerItems[3].PlayerId);
        }

        private RunningCashgame Sut => new RunningCashgame(
            Repos.Bunch,
            Services.CashgameService,
            Repos.Player,
            Repos.User,
            Repos.Location);
    }
}