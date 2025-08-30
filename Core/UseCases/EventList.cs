using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class EventList(
    IEventRepository eventRepository,
    ILocationRepository locationRepository)
    : UseCase<EventList.Request, EventList.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var bunchInfo = request.AccessControl.GetBunchBySlug(request.Slug);

        if (!request.AccessControl.CanListEvents(bunchInfo.Id))
            return Error(new AccessDeniedError());

        var events = await eventRepository.List(bunchInfo.Id);
        var locationIds = events.Select(o => o.LocationId).Where(o => o != null).Distinct().ToList();
        var locations = await locationRepository.List(locationIds!);

        var eventItems = events.OrderByDescending(o => o.StartDate).Select(o => CreateEventItem(o, locations, bunchInfo.Slug)).ToList();

        return Success(new Result(eventItems));
    }

    private static Event CreateEventItem(Entities.Event e, IList<Location> locations, string slug)
    {
        var location = locations.FirstOrDefault(o => o.Id == e.LocationId);
        var locationName = location != null ? location.Name : "";
        var locationId = location?.Id;
        
        return e.HasGames 
            ? new Event(e.Id, slug, e.Name, locationId, locationName, e.StartDate) 
            : new Event(e.Id, slug, e.Name);
    }

    public class Request(IAccessControl accessControl, string slug)
    {
        public IAccessControl AccessControl { get; } = accessControl;
        public string Slug { get; } = slug;
    }

    public class Result(IList<Event> events)
    {
        public IList<Event> Events { get; } = events;
    }

    public class Event(string id, string bunchId, string name)
    {
        public string EventId { get; } = id;
        public string BunchId { get; } = bunchId;
        public string Name { get; } = name;
        public string? LocationId { get; }
        public string? LocationName { get; }
        public Date? StartDate { get; }

        public Event(string id, string bunchId, string name, string? locationId, string? locationName, Date startDate)
            : this(id, bunchId, name)
        {
            LocationId = locationId;
            LocationName = locationName;
            StartDate = startDate;
        }
    }
}