using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class AddLocationTests : TestBase
    {
        [Test]
        public void AddLocation_AllOk_LocationIsAdded()
        {
            const string addedEventName = "added location";

            var request = new AddLocation.Request(TestData.UserA.UserName, TestData.BunchA.Slug, addedEventName);
            Sut.Execute(request);

            Assert.AreEqual(addedEventName, Repos.Location.Added.Name);
        }

        [Test]
        public void AddEvent_InvalidName_ThrowsValidationException()
        {
            const string addedEventName = "";

            var request = new AddLocation.Request(TestData.UserA.UserName, TestData.BunchA.Slug, addedEventName);

            Assert.Throws<ValidationException>(() => Sut.Execute(request));
        }

        private AddLocation Sut => new AddLocation(
            Services.BunchService,
            Services.PlayerService,
            Repos.User,
            Repos.Location);
    }
}