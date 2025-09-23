using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class EventDetailsTests : TestBase
{
    private readonly IEventRepository _eventRepository = Substitute.For<IEventRepository>();
    private readonly ILocationRepository _locationRepository = Substitute.For<ILocationRepository>();
    
    [Fact]
    public async Task EventDetails_NoAccess_ReturnsError()
    {
        var @event = Create.Event();
        _eventRepository.Get(@event.Id).Returns(@event);
        var userBunch = Create.UserBunch(Create.Bunch(), Create.Player());
        
        var request = CreateRequest(userBunch, eventId: @event.Id, canSeeEventDetails: false);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }
    
    [Fact]
    public async Task EventDetails_NameIsSet()
    {
        var @event = Create.Event();
        _eventRepository.Get(@event.Id).Returns(@event);
        var location = Create.Location();
        _locationRepository.Get(location.Id).Returns(location);
        
        var userBunch = Create.UserBunch(Create.Bunch(), Create.Player());
        
        var request = CreateRequest(userBunch, eventId: @event.Id);
        var result = await Sut.Execute(request);

        result.Success.Should().BeTrue();
        result.Data!.Name.Should().Be(@event.Name);
    }

    private EventDetails.Request CreateRequest(
        UserBunch userBunch, 
        string? eventId = null,
        bool? canSeeEventDetails = null) =>
        new(
            new AuthInTest(canSeeEventDetails: canSeeEventDetails ?? true, userBunch: userBunch),
            eventId ?? Create.String());

    private EventDetails Sut => new(_eventRepository, _locationRepository);
}