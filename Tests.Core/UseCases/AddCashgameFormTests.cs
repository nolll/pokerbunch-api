using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    class AddCashgameFormTests : TestBase
    {
        [Test]
        public void AddCashgameOptions_ReturnsResultObject()
        {
            const string slug = TestData.SlugA;
            var result = Sut.Execute(new AddCashgameForm.Request(TestData.UserNameA, slug));

            Assert.IsInstanceOf<AddCashgameForm.Result>(result);
        }

        [Test]
        public void AddCashgameOptions_WithRunningCashgame_ThrowsException()
        {
            Repos.Cashgame.SetupRunningGame();

            const string slug = TestData.SlugA;

            Assert.Throws<CashgameRunningException>(() => Sut.Execute(new AddCashgameForm.Request(TestData.UserNameA, slug)));
        }

        [Test]
        public void AddCashgameOptions_LocationsAreSet()
        {
            const string slug = TestData.SlugA;
            var result = Sut.Execute(new AddCashgameForm.Request(TestData.UserNameA, slug));

            Assert.AreEqual(4, result.Locations.Count);
        }

        private AddCashgameForm Sut
        {
            get
            {
                return new AddCashgameForm(
                    Services.BunchService,
                    Services.CashgameService,
                    Services.UserService,
                    Services.PlayerService,
                    Services.LocationService,
                    Services.EventService);
            }
        }
    }
}