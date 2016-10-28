using System.Linq;
using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class AddCashgameTests : TestBase
    {
        [Test]
        public void AddCashgame_SlugIsSet()
        {
            var request = CreateRequest(TestData.LocationIdA);
            var result = Sut.Execute(request);

            Assert.AreEqual(TestData.SlugA, result.Slug);
        }

        [Test]
        public void AddCashgame_WithLocation_GameIsAdded()
        {
            var request = CreateRequest(TestData.LocationIdA);
            Sut.Execute(request);

            Assert.IsNotNull(Repos.Cashgame.Added);
        }

        [Test]
        public void AddCashgame_WithEventId_GameIsAddedToEvent()
        {
            var request = CreateRequest(TestData.LocationIdA, 2);
            Sut.Execute(request);

            Assert.AreEqual(1, Repos.Event.AddedCashgameId);
        }

        [Test]
        public void AddCashgame_WithoutLocation_ThrowsValidationException()
        {
            var request = CreateRequest();

            var ex = Assert.Throws<ValidationException>(() => Sut.Execute(request));
            Assert.AreEqual(1, ex.Messages.Count());
        }

        private static AddCashgame.Request CreateRequest(int locationId = 0, int eventId = 0)
        {
            return new AddCashgame.Request(TestData.UserNameA, TestData.SlugA, locationId, eventId);
        }

        private AddCashgame Sut => new AddCashgame(
            Repos.Bunch,
            Services.CashgameService,
            Repos.User,
            Repos.Player,
            Repos.Location,
            Repos.Event);
    }
}
