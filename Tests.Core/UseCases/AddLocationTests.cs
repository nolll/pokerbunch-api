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
    public async Task AddLocation_AllOk_LocationIsAdded()
    {
        const string locationName = "added location";

        var bunch = Create.Bunch();
        var userBunch = Create.UserBunch(bunch.Id, bunch.Slug, "", "", "", Role.Manager);
        var request = new AddLocation.Request(new AuthInTest(canAddLocation: true, userBunch: userBunch), bunch.Slug, locationName);
        await Sut.Execute(request);

        await _locationRepository.Received().Add(Arg.Is<Location>(o => o.Name == locationName));
    }

    [Fact]
    public async Task AddEvent_InvalidName_ReturnsError()
    {
        const string addedEventName = "";

        var bunch = Create.Bunch();
        var request = new AddLocation.Request(new AuthInTest(canAddLocation: true), bunch.Slug, addedEventName);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    private AddLocation Sut => new(_locationRepository);
}