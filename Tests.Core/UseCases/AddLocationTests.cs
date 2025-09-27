using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class AddLocationTests : TestBase
{
    private readonly ILocationRepository _locationRepository = Substitute.For<ILocationRepository>();
    
    [Fact]
    public async Task AddEvent_NoAccess_ReturnsError()
    {
        var bunch = Create.Bunch();
        
        var request = CreateRequest(slug: bunch.Slug, locationName: "added location", canAddLocation: false);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }
    
    [Fact]
    public async Task AddEvent_InvalidName_ReturnsError()
    {
        var bunch = Create.Bunch();
        
        var request = CreateRequest(slug: bunch.Slug, locationName: "");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }
    
    [Fact]
    public async Task AddLocation_AllOk_LocationIsAdded()
    {
        const string locationName = "added location";
        var bunch = Create.Bunch();

        var request = CreateRequest(bunch.Slug, locationName: locationName);
        await Sut.Execute(request);

        await _locationRepository.Received().Add(Arg.Is<Location>(o => o.Name == locationName));
    }

    private AddLocation.Request CreateRequest(string? slug = null, string? locationName = null, bool? canAddLocation = null)
    {
        return new AddLocation.Request(
            new AuthInTest(canAddLocation: canAddLocation ?? true), 
            slug ?? Create.String(), 
            locationName ?? Create.String());
    }

    private AddLocation Sut => new(_locationRepository);
}