using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    class MatrixTests : TestBase
    {
        [Test]
        public void Matrix_WithTwoGames_GameItemsAreCorrectAndSortedByDateDescending()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.AreEqual(2, result.GameItems.Count);
            Assert.AreEqual("2002-02-02", result.GameItems[0].Date.IsoString);
            Assert.AreEqual(2, result.GameItems[0].Id);
            Assert.AreEqual("2001-01-01", result.GameItems[1].Date.IsoString);
            Assert.AreEqual(1, result.GameItems[1].Id);
        }

        [Test]
        public void Matrix_WithTwoGamesOnTheSameYear_SpansMultipleYearsIsFalse()
        {
            Repos.Cashgame.SetupSingleYear();

            var result = Sut.Execute(CreateRequest());

            Assert.IsFalse(result.SpansMultipleYears);
        }

        [Test]
        public void Matrix_WithTwoGamesOnDifferentYears_SpansMultipleYearsIsTrue()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.IsTrue(result.SpansMultipleYears);
        }

        private static Matrix.Request CreateRequest(int? year = null)
        {
            return new Matrix.Request(TestData.UserNameA, TestData.SlugA, year);
        }

        private Matrix Sut => new Matrix(
            Services.BunchService,
            Services.CashgameService,
            Services.PlayerService,
            Repos.User,
            Repos.Event);
    }
}
