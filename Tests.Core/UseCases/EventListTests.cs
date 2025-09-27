using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class EventListTests : TestBase
{
    private readonly IEventRepository _eventRepository = Substitute.For<IEventRepository>();
    private readonly ILocationRepository _locationRepository = Substitute.For<ILocationRepository>();

    [Fact]
    public async Task EventList_NoAccess_ReturnsError()
    {
        var request = CreateRequest(canListEvents: false);
        var result = await Sut.Execute(request);

        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }
    
    [Fact]
    public async Task EventList_ReturnsAllEvents()
    {
        var bunch = Create.Bunch();
        var event1 = Create.Event(bunchSlug: bunch.Slug, startDate: new Date(2025, 2, 1), endDate: new Date(2025, 2, 2));
        var event2 = Create.Event(bunchSlug: bunch.Slug, startDate: new Date(2025, 1, 1), endDate: new Date(2025, 1, 2));
        Event[] events = [event1, event2];
        _eventRepository.List(bunch.Slug).Returns(events.OrderByDescending(o => o.StartDate).ToList());

        var request = CreateRequest(bunch.Slug);
        var result = await Sut.Execute(request);

        result.Success.Should().BeTrue();
        result.Data!.Events.Count.Should().Be(2);
        result.Data!.Events[0].EventId.Should().Be(event1.Id);
        result.Data!.Events[0].Name.Should().Be(event1.Name);
        result.Data!.Events[0].StartDate.Should().Be(event1.StartDate);
        result.Data!.Events[1].EventId.Should().Be(event2.Id);
        result.Data!.Events[1].Name.Should().Be(event2.Name);
        result.Data!.Events[1].StartDate.Should().Be(event2.StartDate);
    }

    private EventList.Request CreateRequest(string? slug = null, bool? canListEvents = null) =>
        new(
            new AuthInTest(canListEvents: canListEvents ?? true), 
            slug ?? Create.String());

    private EventList Sut => new(_eventRepository, _locationRepository);
}