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
        var request = new GetLocation.Request(new AuthInTest(canSeeLocation: true, currentBunch: currentBunch), TestData.LocationIdA);
        var result = await Sut.Execute(request);

        result.Data!.Id.Should().Be(TestData.BunchA.Id);
        result.Data!.Name.Should().Be(TestData.LocationNameA);
        result.Data!.Slug.Should().Be(TestData.BunchA.Slug);
    }

    private GetLocation Sut => new(Deps.Location);
}