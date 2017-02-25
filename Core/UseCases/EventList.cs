using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class EventList
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

        public Result Execute(Request request)
        {
            var bunch = _bunchRepository.GetBySlug(request.Slug);
            var user = _userRepository.Get(request.UserName);
            var player = _playerRepository.Get(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var events = _eventRepository.List(bunch.Id);
            var locationIds = events.Select(o => o.LocationId).Distinct().ToList();
            var locations = _locationRepository.List(locationIds);

            var eventItems = events.OrderByDescending(o => o.StartDate).Select(o => CreateEventItem(o, locations, bunch.Slug)).ToList();

            return new Result(eventItems);
        }

        private static Event CreateEventItem(Entities.Event e, IList<Location> locations, string slug)
        {
            var location = locations.FirstOrDefault(o => o.Id == e.LocationId);
            var locationName = location != null ? location.Name : "";
            var locationId = location?.Id ?? 0;
            if(e.HasGames)
                return new Event(e.Id, slug, e.Name, locationId, locationName, e.StartDate, e.EndDate);
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
            public IList<Event> Events { get; private set; }

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
            public Date EndDate { get; }
            public bool HasGames { get; }

            public Event(int id, string bunchId, string name)
            {
                EventId = id;
                BunchId = bunchId;
                Name = name;
                HasGames = false;
            }
            
            public Event(int id, string bunchId, string name, int locationId, string locationName, Date startDate, Date endDate)
                : this(id, bunchId, name)
            {
                LocationId = locationId;
                LocationName = locationName;
                StartDate = startDate;
                EndDate = endDate;
                HasGames = true;
            }
        }
    }
}