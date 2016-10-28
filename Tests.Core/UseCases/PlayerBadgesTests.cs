using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    class PlayerBadgesTests : TestBase
    {
        [Test]
        public void PlayerBadges_ZeroGames_AllBadgesAreFalse()
        {
            Deps.Cashgame.ClearList();

            var result = Sut.Execute(CreateRequest());

            Assert.IsFalse(result.PlayedOneGame);
            Assert.IsFalse(result.PlayedTenGames);
            Assert.IsFalse(result.Played50Games);
            Assert.IsFalse(result.Played100Games);
            Assert.IsFalse(result.Played200Games);
            Assert.IsFalse(result.Played500Games);
        }

        [Test]
        public void PlayerBadges_OneGame_PlayedOneGameIsTrue()
        {
            var result = Sut.Execute(CreateRequest());

            Assert.IsTrue(result.PlayedOneGame);
        }

        [Test]
        public void PlayerBadges_TenGames_PlayedTenGamesIsTrue()
        {
            Deps.Cashgame.SetupGameCount(10);

            var result = Sut.Execute(CreateRequest());

            Assert.IsTrue(result.PlayedTenGames);
        }

        [Test]
        public void PlayerBadges_50Games_Played50GamesIsTrue()
        {
            Deps.Cashgame.SetupGameCount(50);

            var result = Sut.Execute(CreateRequest());

            Assert.IsTrue(result.Played50Games);
        }

        [Test]
        public void PlayerBadges_100Games_Played100GamesIsTrue()
        {
            Deps.Cashgame.SetupGameCount(100);

            var result = Sut.Execute(CreateRequest());

            Assert.IsTrue(result.Played100Games);
        }

        [Test]
        public void PlayerBadges_200Games_Played200GamesIsTrue()
        {
            Deps.Cashgame.SetupGameCount(200);

            var result = Sut.Execute(CreateRequest());

            Assert.IsTrue(result.Played200Games);
        }

        [Test]
        public void PlayerBadges_500Games_Played500GamesIsTrue()
        {
            Deps.Cashgame.SetupGameCount(500);

            var result = Sut.Execute(CreateRequest());

            Assert.IsTrue(result.Played500Games);
        }

        private PlayerBadges.Request CreateRequest()
        {
            return new PlayerBadges.Request(TestData.UserNameA, TestData.PlayerIdA);
        }

        private PlayerBadges Sut => new PlayerBadges(
            Deps.Bunch,
            Deps.Cashgame,
            Deps.Player,
            Deps.User);
    }
}
