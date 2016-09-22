using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    class CashgameChartTests : TestBase
    {
        [Test]
        public void CashgameChart_GameDataIsCorrect()
        {
            var request = new CashgameChart.Request(TestData.UserNameA, TestData.SlugA, null);
            var result = Sut.Execute(request);

            Assert.AreEqual(2, result.GameItems.Count);

            Assert.AreEqual("2001-01-01", result.GameItems[0].Date.IsoString);
            Assert.AreEqual(2, result.GameItems[0].Winnings.Count);
            Assert.AreEqual(-150, result.GameItems[0].Winnings[TestData.PlayerIdA]);
            Assert.AreEqual(150, result.GameItems[0].Winnings[TestData.PlayerIdB]);
            Assert.AreEqual(2, result.GameItems[1].Winnings.Count);
            Assert.AreEqual(200, result.GameItems[1].Winnings[TestData.PlayerIdA]);
            Assert.AreEqual(-200, result.GameItems[1].Winnings[TestData.PlayerIdB]);
        }

        [Test]
        public void CashgameChart_PlayerDataIsCorrect()
        {
            var request = new CashgameChart.Request(TestData.UserNameA, TestData.SlugA, null);
            var result = Sut.Execute(request);

            Assert.AreEqual(2, result.PlayerItems.Count);
            Assert.AreEqual(TestData.PlayerIdA, result.PlayerItems[0].Id);
            Assert.AreEqual(TestData.PlayerNameA, result.PlayerItems[0].Name);
            Assert.AreEqual(TestData.PlayerIdB, result.PlayerItems[1].Id);
            Assert.AreEqual(TestData.PlayerNameB, result.PlayerItems[1].Name);
        }

        private CashgameChart Sut
        {
            get
            {
                return new CashgameChart(
                    Services.BunchService,
                    Services.CashgameService,
                    Services.PlayerService,
                    Services.UserService);
            }
        }
    }
}
