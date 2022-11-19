using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

public class LocationDetailsTests : TestBase
{
    [Test]
    public async Task LocationDetails_AllPropertiesAreSet()
    {
        var request = new GetLocation.Request(TestData.UserA.UserName, TestData.LocationIdA);
        var result = await Sut.Execute(request);

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