using Core.Entities;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class LocationDetailsTests : TestBase
{
    private readonly ILocationRepository _locationRepository = Substitute.For<ILocationRepository>();

    [Test]
    public async Task LocationDetails_AllPropertiesAreSet()
    {
        var location = Create.Type<Location>();
        var userBunch = Create.UserBunch();
        var request = new GetLocation.Request(new AuthInTest(canSeeLocation: true, userBunch: userBunch), location.Id);
        _locationRepository.Get(location.Id).Returns(location);
        
        var result = await Sut.Execute(request);
        
        result.Data!.Name.Should().Be(location.Name);
        result.Data!.Slug.Should().Be(userBunch.Slug);
    }

    private GetLocation Sut => new(_locationRepository);
}