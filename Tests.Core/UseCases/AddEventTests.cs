using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class AddEventTests : TestBase
    {
        [Test]
        public void AddEvent_AllOk_EventIsAdded()
        {
            const string addedEventName = "added event";

            var request = new AddEvent.Request(TestData.UserA.UserName, TestData.BunchA.Slug, addedEventName);
            Sut.Execute(request);

            Assert.AreEqual(addedEventName, Repos.Event.Added.Name);
        }

        [Test]
        public void AddEvent_InvalidName_ThrowsValidationException()
        {
            const string addedEventName = "";

            var request = new AddEvent.Request(TestData.UserA.UserName, TestData.BunchA.Slug, addedEventName);

            Assert.Throws<ValidationException>(() => Sut.Execute(request));
        }

        private AddEvent Sut => new AddEvent(
            Repos.Bunch,
            Services.PlayerService,
            Repos.User,
            Repos.Event);
    }
}