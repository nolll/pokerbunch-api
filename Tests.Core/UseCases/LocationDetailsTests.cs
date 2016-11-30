using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class LocationDetailsTests : TestBase
    {
        [Test]
        public void LocationDetails_AllPropertiesAreSet()
        {
            var request = new GetLocation.Request(TestData.UserA.UserName, TestData.LocationIdA);
            var result = Sut.Execute(request);

            Assert.AreEqual(TestData.BunchA.Id, result.Id);
            Assert.AreEqual(TestData.LocationNameA, result.Name);
            Assert.AreEqual(TestData.BunchA.Slug, result.Slug);
        }

        private GetLocation Sut => new GetLocation(
            Deps.Location,
            Deps.User,
            Deps.Player,
            Deps.Bunch);
    }
}