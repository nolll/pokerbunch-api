using System.Collections.Generic;
using System.Linq;
using Core.Exceptions;
using Core.Services;

namespace Core.UseCases
{
    public class AddCashgameForm
    {
        private readonly BunchService _bunchService;
        private readonly CashgameService _cashgameService;
        private readonly UserService _userService;
        private readonly PlayerService _playerService;
        private readonly LocationService _locationService;
        private readonly EventService _eventService;

        public AddCashgameForm(BunchService bunchService, CashgameService cashgameService, UserService userService, PlayerService playerService, LocationService locationService, EventService eventService)
        {
            _bunchService = bunchService;
            _cashgameService = cashgameService;
            _userService = userService;
            _playerService = playerService;
            _locationService = locationService;
            _eventService = eventService;
        }

        public Result Execute(Request request)
        {
            var bunch = _bunchService.GetBySlug(request.Slug);
            var user = _userService.GetByNameOrEmail(request.UserName);
            var player = _playerService.GetByUserId(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var runningGame = _cashgameService.GetRunning(bunch.Id);
            if (runningGame != null)
            {
                throw new CashgameRunningException();
            }
            var locations = _locationService.ListByBunch(bunch.Id);
            var locationItems = locations.Select(o => new LocationItem(o.Id, o.Name)).ToList();
            var events = _eventService.GetByBunch(bunch.Id);
            var eventItems = events.Select(o => new EventItem(o.Id, o.Name)).ToList();
            return new Result(locationItems, eventItems);
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
            public IList<LocationItem> Locations { get; private set; }
            public IList<EventItem> Events { get; private set; }

            public Result(IList<LocationItem> locations, IList<EventItem> events)
            {
                Locations = locations;
                Events = events;
            }
        }

        public class LocationItem
        {
            public int Id { get; private set; }
            public string Name { get; private set; }

            public LocationItem(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        public class EventItem
        {
            public int Id { get; private set; }
            public string Name { get; private set; }

            public EventItem(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }
    }
}