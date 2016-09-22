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

            Assert.IsNull(Repos.Cashgame.Updated);
        }

        [Test]
        public void EndGame_WithRunningGame_EndsGame()
        {
            Repos.Cashgame.SetupRunningGame();

            var request = new EndCashgame.Request(TestData.UserNameA, TestData.SlugA);
            Sut.Execute(request);

            Assert.AreEqual(TestData.CashgameIdC, Repos.Cashgame.Updated.Id);
            Assert.AreEqual(GameStatus.Finished, Repos.Cashgame.Updated.Status);
        }

        private EndCashgame Sut
        {
            get
            {
                return new EndCashgame(
                    Services.BunchService,
                    Services.CashgameService,
                    Services.UserService,
                    Services.PlayerService);
            }
        }
    }
}