using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

public class LocationDetailsTests : TestBase
{
    [Test]
    public void LocationDetails_AllPropertiesAreSet()
    {
        var request = new GetLocation.Request(TestData.UserA.UserName, TestData.LocationIdA);
        var result = Sut.Execute(request);

        Assert.AreEqual(TestData.BunchA.Id, result.Data.Id);
        Assert.AreEqual(TestData.LocationNameA, result.Data.Name);
        Assert.AreEqual(TestData.BunchA.Slug, result.Data.Slug);
    }

    private GetLocation Sut => new(
        Deps.Location,
        Deps.User,
        Deps.Player,
        Deps.Bunch);
}