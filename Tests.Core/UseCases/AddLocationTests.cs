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
    
    [Test]
    public async Task AddLocation_AllOk_LocationIsAdded()
    {
        const string locationName = "added location";

        var userBunch = Create.UserBunch(TestData.BunchA.Id, TestData.BunchA.Slug, "", "", "", Role.Manager);
        var request = new AddLocation.Request(new AuthInTest(canAddLocation: true, userBunch: userBunch), TestData.BunchA.Slug, locationName);
        await Sut.Execute(request);

        await _locationRepository.Received().Add(Arg.Is<Location>(o => o.Name == locationName));
    }

    [Test]
    public async Task AddEvent_InvalidName_ReturnsError()
    {
        const string addedEventName = "";

        var request = new AddLocation.Request(new AuthInTest(canAddLocation: true), TestData.BunchA.Slug, addedEventName);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    private AddLocation Sut => new(_locationRepository);
}