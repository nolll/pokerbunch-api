using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class EventList(
    IBunchRepository bunchRepository,
    IEventRepository eventRepository,
    IUserRepository userRepository,
    IPlayerRepository playerRepository,
    ILocationRepository locationRepository)
    : UseCase<EventList.Request, EventList.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var bunch = await bunchRepository.GetBySlug(request.Slug);
        var user = await userRepository.GetByUserName(request.UserName);
        var player = await playerRepository.Get(bunch.Id, user.Id);

        if (!AccessControl.CanListEvents(user, player))
            return Error(new AccessDeniedError());

        var events = await eventRepository.List(bunch.Id);
        var locationIds = events.Select(o => o.LocationId).Where(o => o != null).Distinct().ToList();
        var locations = await locationRepository.List(locationIds!);

        var eventItems = events.OrderByDescending(o => o.StartDate).Select(o => CreateEventItem(o, locations, bunch.Slug)).ToList();

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

    public class Request(string userName, string slug)
    {
        public string UserName { get; } = userName;
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