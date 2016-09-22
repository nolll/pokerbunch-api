using System;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class CashgameDetailsChartTests : TestBase
    {
        [Test]
        public void PlayerItems_EndedGame_TwoPlayersWithTwoAndThreeCheckpoints()
        {
            var request = new CashgameDetailsChart.Request(TestData.UserNameA, DateTime.Now, TestData.CashgameIdA);
            var result = Sut.Execute(request);

            Assert.AreEqual(2, result.PlayerItems.Count);
            Assert.AreEqual(TestData.PlayerNameA, result.PlayerItems[0].Name);
            Assert.AreEqual(2, result.PlayerItems[0].Results.Count);
            Assert.AreEqual(0, result.PlayerItems[0].Results[0].Winnings);
            Assert.AreEqual(-150, result.PlayerItems[0].Results[1].Winnings);
            Assert.AreEqual(3, result.PlayerItems[1].Results.Count);
            Assert.AreEqual(0, result.PlayerItems[1].Results[0].Winnings);
            Assert.AreEqual(50, result.PlayerItems[1].Results[1].Winnings);
            Assert.AreEqual(150, result.PlayerItems[1].Results[2].Winnings);
        }

        private CashgameDetailsChart Sut
        {
            get
            {
                return new CashgameDetailsChart(
                    Services.BunchService,
                    Services.CashgameService,
                    Services.PlayerService,
                    Services.UserService);
            }
        }
    }
}