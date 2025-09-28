using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class LocationListTests : TestBase
{
    private readonly ILocationRepository _locationRepository = Substitute.For<ILocationRepository>();

    [Fact]
    public async Task LocationList_NoAccess_ReturnsError()
    {
        var request = CreateRequest(canListLocations: false);
        var result = await Sut.Execute(request);

        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }
    
    [Fact]
    public async Task LocationList_ReturnsAllLocations()
    {
        var bunch = Create.Bunch();
        var location1 = Create.Location(bunchSlug: bunch.Slug, name: "a");
        var location2 = Create.Location(bunchSlug: bunch.Slug, name: "b");
        Location[] locations = [location1, location2];
        _locationRepository.List(bunch.Slug).Returns(locations.OrderBy(o => o.Name).ToList());

        var request = CreateRequest(bunch.Slug);
        var result = await Sut.Execute(request);

        result.Success.Should().BeTrue();
        result.Data!.Locations.Count.Should().Be(2);
        result.Data!.Locations[0].Id.Should().Be(location1.Id);
        result.Data!.Locations[0].Name.Should().Be(location1.Name);
        result.Data!.Locations[1].Id.Should().Be(location2.Id);
        result.Data!.Locations[1].Name.Should().Be(location2.Name);
    }

    private GetLocationList.Request CreateRequest(string? slug = null, bool? canListLocations = null) =>
        new(
            new AuthInTest(canListLocations: canListLocations ?? true), 
            slug ?? Create.String());

    private GetLocationList Sut => new(_locationRepository);
}