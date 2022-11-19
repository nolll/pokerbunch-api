using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class EventList : UseCase<EventList.Request, EventList.Result>
{
    private readonly IBunchRepository _bunchRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ILocationRepository _locationRepository;

    public EventList(IBunchRepository bunchRepository, IEventRepository eventRepository, IUserRepository userRepository, IPlayerRepository playerRepository, ILocationRepository locationRepository)
    {
        _bunchRepository = bunchRepository;
        _eventRepository = eventRepository;
        _userRepository = userRepository;
        _playerRepository = playerRepository;
        _locationRepository = locationRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var bunch = await _bunchRepository.GetBySlug(request.Slug);
        var user = await _userRepository.Get(request.UserName);
        var player = await _playerRepository.Get(bunch.Id, user.Id);

        if (!AccessControl.CanListEvents(user, player))
            return Error(new AccessDeniedError());

        var events = await _eventRepository.List(bunch.Id);
        var locationIds = events.Select(o => o.LocationId).Distinct().ToList();
        var locations = await _locationRepository.List(locationIds);

        var eventItems = events.OrderByDescending(o => o.StartDate).Select(o => CreateEventItem(o, locations, bunch.Slug)).ToList();

        return Success(new Result(eventItems));
    }

    private static Event CreateEventItem(Entities.Event e, IList<Location> locations, string slug)
    {
        var location = locations.FirstOrDefault(o => o.Id == e.LocationId);
        var locationName = location != null ? location.Name : "";
        var locationId = location?.Id ?? 0;
        if(e.HasGames)
            return new Event(e.Id, slug, e.Name, locationId, locationName, e.StartDate);
        return new Event(e.Id, slug, e.Name);
    }

    public class Request
    {
        public string UserName { get; }
        public string Slug { get; }

        public Request(string userName, string slug)
        {
            UserName = userName;
            Slug = slug;
        }
    }

    public class Result
    {
        public IList<Event> Events { get; }

        public Result(IList<Event> events)
        {
            Events = events;
        }
    }

    public class Event
    {
        public int EventId { get; }
        public string BunchId { get; }
        public string Name { get; }
        public int LocationId { get; }
        public string LocationName { get; }
        public Date StartDate { get; }

        public Event(int id, string bunchId, string name)
        {
            EventId = id;
            BunchId = bunchId;
            Name = name;
        }
            
        public Event(int id, string bunchId, string name, int locationId, string locationName, Date startDate)
            : this(id, bunchId, name)
        {
            LocationId = locationId;
            LocationName = locationName;
            StartDate = startDate;
        }
    }
}