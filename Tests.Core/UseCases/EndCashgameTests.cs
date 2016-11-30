using Core.Entities;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class EndCashgameTests : TestBase
    {
        [Test]
        public void EndGame_WithoutRunningGame_DoesNotEndGame()
        {
            var request = new EndCashgame.Request(TestData.UserNameA, TestData.SlugA);
            Sut.Execute(request);

            Assert.IsNull(Deps.Cashgame.Updated);
        }

        [Test]
        public void EndGame_WithRunningGame_EndsGame()
        {
            Deps.Cashgame.SetupRunningGame();

            var request = new EndCashgame.Request(TestData.UserNameA, TestData.SlugA);
            Sut.Execute(request);

            Assert.AreEqual(TestData.CashgameIdC, Deps.Cashgame.Updated.Id);
            Assert.AreEqual(GameStatus.Finished, Deps.Cashgame.Updated.Status);
        }

        private EndCashgame Sut => new EndCashgame(
            Deps.Bunch,
            Deps.Cashgame,
            Deps.User,
            Deps.Player);
    }
}