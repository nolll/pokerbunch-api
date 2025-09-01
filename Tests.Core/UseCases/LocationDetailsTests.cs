using Core.Entities;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class LocationDetailsTests : TestBase
{
    [Test]
    public async Task LocationDetails_AllPropertiesAreSet()
    {
        var currentBunch = new CurrentBunch(TestData.BunchA.Id, TestData.BunchA.Slug, TestData.BunchA.DisplayName, "", "", Role.None);
        var request = new GetLocation.Request(new PrincipalInTest(canSeeLocation: true, currentBunch: currentBunch), TestData.LocationIdA);
        var result = await Sut.Execute(request);

        Assert.That(result.Data?.Id, Is.EqualTo(TestData.BunchA.Id));
        Assert.That(result.Data?.Name, Is.EqualTo(TestData.LocationNameA));
        Assert.That(result.Data?.Slug, Is.EqualTo(TestData.BunchA.Slug));
    }

    private GetLocation Sut => new(Deps.Location);
}