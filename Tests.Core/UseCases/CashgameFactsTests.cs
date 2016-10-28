using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    class CashgameFactsTests : TestBase
    {
        [Test]
        public void Execute_ReturnsCashgameFactResult()
        {
            var request = new CashgameFacts.Request(TestData.UserNameA, TestData.SlugA, null);
            var result = Sut.Execute(request);

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetFactsResult_AllPropertiesAreSet()
        {
            var request = new CashgameFacts.Request(TestData.UserNameA, TestData.SlugA, null);
            var result = Sut.Execute(request);

            Assert.AreEqual(2, result.GameCount);
            Assert.AreEqual(154, result.TotalTimePlayed.Minutes);
            Assert.AreEqual(1000, result.Turnover.Amount);
            Assert.AreEqual(-350, result.WorstResult.Amount.Amount);
            Assert.AreEqual(+350, result.BestResult.Amount.Amount);
            Assert.AreEqual(-200, result.WorstTotalResult.Amount.Amount);
            Assert.AreEqual(200, result.BestTotalResult.Amount.Amount);
            Assert.AreEqual(600, result.BiggestBuyin.Amount.Amount);
            Assert.AreEqual(600, result.BiggestCashout.Amount.Amount);
            Assert.AreEqual(152, result.MostTimePlayed.Time.Minutes);
        }

        private CashgameFacts Sut => new CashgameFacts(
            Repos.Bunch,
            Services.CashgameService,
            Repos.Player,
            Repos.User);
    }
}
