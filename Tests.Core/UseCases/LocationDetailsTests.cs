using Core.Entities;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class LocationDetailsTests : TestBase
{
    private readonly ILocationRepository _locationRepository = Substitute.For<ILocationRepository>();

    [Fact]
    public async Task LocationDetails_AllPropertiesAreSet()
    {
        var bunch = Create.Bunch();
        var location = Create.Type<Location>();
        _locationRepository.Get(location.Id).Returns(location);

        var request = CreateRequest(bunch.Id, bunch.Slug, location.Id);
        var result = await Sut.Execute(request);
        
        result.Data!.Name.Should().Be(location.Name);
        result.Data!.Slug.Should().Be(bunch.Slug);
    }

    private GetLocation.Request CreateRequest(string? bunchId = null, string? slug = null, string? locationId = null, bool? canSeeLocation = null)
    {
        var userBunch = Create.UserBunch(bunchId, slug);
        return new GetLocation.Request(
            new AuthInTest(canSeeLocation: true, userBunch: userBunch),
            locationId ?? Create.String());
    }

    private GetLocation Sut => new(_locationRepository);
}