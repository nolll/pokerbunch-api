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
        private readonly PlayerService _playerService;
        private readonly ILocationRepository _locationRepository;

        public EventList(IBunchRepository bunchRepository, IEventRepository eventRepository, IUserRepository userRepository, PlayerService playerService, ILocationRepository locationRepository)
        {
            _bunchRepository = bunchRepository;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _playerService = playerService;
            _locationRepository = locationRepository;
        }

        public Result Execute(Request request)
        {
            var bunch = _bunchRepository.GetBySlug(request.Slug);
            var user = _userRepository.Get(request.UserName);
            var player = _playerService.GetByUserId(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var events = _eventRepository.List(bunch.Id);
            var locationIds = events.Select(o => o.LocationId).Distinct().ToList();
            var locations = _locationRepository.List(locationIds);

            var eventItems = events.OrderByDescending(o => o.StartDate).Select(o => CreateEventItem(o, locations)).ToList();

            return new Result(eventItems);
        }

        private static Item CreateEventItem(Event e, IList<Location> locations)
        {
            var location = locations.FirstOrDefault(o => o.Id == e.LocationId);
            var locationName = location != null ? location.Name : "";
            if(e.HasGames)
                return new Item(e.Id, e.Name, locationName, e.StartDate, e.EndDate);
            return new Item(e.Id, e.Name);
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
            public IList<Item> Events { get; private set; }

            public Result(IList<Item> events)
            {
                Events = events;
            }
        }

        public class Item
        {
            public int EventId { get; private set; }
            public string Name { get; private set; }
            public string Location { get; private set; }
            public Date StartDate { get; private set; }
            public Date EndDate { get; private set; }
            public bool HasGames { get; private set; }

            public Item(int id, string name)
            {
                EventId = id;
                Name = name;
                HasGames = false;
            }
            
            public Item(int id, string name, string location, Date startDate, Date endDate)
                : this(id, name)
            {
                Location = location;
                StartDate = startDate;
                EndDate = endDate;
                HasGames = true;
            }
        }
    }
}