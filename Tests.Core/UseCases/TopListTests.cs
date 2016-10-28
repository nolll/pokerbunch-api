using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class TopListTests : TestBase
    {
        [Test]
        public void TopList_ReturnsTopListItems()
        {
            var request = new TopList.Request(TestData.UserNameA, TestData.SlugA, null);
            var result = Sut.Execute(request);

            Assert.AreEqual(2, result.Items.Count);
            Assert.AreEqual(null, result.Year);
            Assert.AreEqual(TestData.SlugA, result.Slug);
        }

        [Test]
        public void TopList_ItemHasCorrectValues()
        {
            var request = new TopList.Request(TestData.UserNameA, TestData.SlugA, null);
            var result = Sut.Execute(request);

            Assert.AreEqual(1, result.Items[0].Rank);
            Assert.AreEqual(400, result.Items[0].Buyin.Amount);
            Assert.AreEqual(600, result.Items[0].Cashout.Amount);
            Assert.AreEqual(2, result.Items[0].GamesPlayed);
            Assert.AreEqual(152, result.Items[0].TimePlayed.Minutes);
            Assert.AreEqual(TestData.PlayerNameA, result.Items[0].Name);
            Assert.AreEqual(1, result.Items[0].PlayerId);
            Assert.AreEqual(200, result.Items[0].Winnings.Amount);
            Assert.AreEqual(79, result.Items[0].WinRate.Amount);
        }

        [Test]
        public void TopList_HighestWinningsIsFirst()
        {
            var request = new TopList.Request(TestData.UserNameA, TestData.SlugA, null);
            var result = Sut.Execute(request);

            Assert.AreEqual(200, result.Items[0].Winnings.Amount);
            Assert.AreEqual(-200, result.Items[1].Winnings.Amount);
        }

        private TopList Sut => new TopList(
            Deps.Bunch,
            Deps.Cashgame,
            Deps.Player,
            Deps.User);
    }
}