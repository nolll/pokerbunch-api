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

        Assert.That(result.Data?.Id, Is.EqualTo(TestData.BunchA.Id));
        Assert.That(result.Data?.Name, Is.EqualTo(TestData.LocationNameA));
        Assert.That(result.Data?.Slug, Is.EqualTo(TestData.BunchA.Slug));
    }

    private GetLocation Sut => new(
        Deps.Location,
        Deps.User,
        Deps.Player,
        Deps.Bunch);
}