using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class DeleteCashgameTests : TestBase
    {
        [Test]
        public void DeleteCashgame_GameHasResults_ThrowsCashgameHasResultsException()
        {
            var request = new DeleteCashgame.Request(TestData.ManagerUser.UserName, TestData.CashgameIdA);

            Assert.Throws<CashgameHasResultsException>(() => Sut.Execute(request));
        }

        [Test]
        public void DeleteCashgame_GameHasNoResults_DeletesGame()
        {
            Repos.Cashgame.SetupEmptyGame();

            var request = new DeleteCashgame.Request(TestData.ManagerUser.UserName, TestData.CashgameIdA);

            Sut.Execute(request);

            Assert.AreEqual(TestData.CashgameIdA, Repos.Cashgame.Deleted);
        }

        [Test]
        public void DeleteCashgame_GameHasNoResults_ReturnUrlIsSet()
        {
            Repos.Cashgame.SetupEmptyGame();

            var request = new DeleteCashgame.Request(TestData.ManagerUser.UserName, TestData.CashgameIdA);

            var result = Sut.Execute(request);

            Assert.AreEqual(TestData.SlugA, result.Slug);
        }

        private DeleteCashgame Sut => new DeleteCashgame(
            Services.CashgameService,
            Repos.Bunch,
            Repos.User,
            Repos.Player);
    }
}