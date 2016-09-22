using System.Collections.Generic;
using System.Linq;
using Core.Services;

namespace Core.UseCases
{
    public class EditCashgameForm
    {
        private readonly BunchService _bunchService;
        private readonly CashgameService _cashgameService;
        private readonly UserService _userService;
        private readonly PlayerService _playerService;
        private readonly LocationService _locationService;
        private readonly EventService _eventService;

        public EditCashgameForm(BunchService bunchService, CashgameService cashgameService, UserService userService, PlayerService playerService, LocationService locationService, EventService eventService)
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
            var cashgame = _cashgameService.GetById(request.Id);
            var bunch = _bunchService.Get(cashgame.BunchId);
            var user = _userService.GetByNameOrEmail(request.UserName);
            var player = _playerService.GetByUserId(cashgame.BunchId, user.Id);
            RequireRole.Manager(user, player);

            var locations = _locationService.GetByBunch(cashgame.BunchId);
            var locationItems = locations.Select(o => new LocationItem(o.Id, o.Name)).ToList();

            var events = _eventService.GetByBunch(bunch.Id);
            var eventItems = events.Select(o => new EventItem(o.Id, o.Name)).ToList();
            var selectedEvent = _eventService.GetByCashgame(cashgame.Id);
            var selectedEventId = selectedEvent != null ? selectedEvent.Id : 0;

            return new Result(cashgame.DateString, cashgame.Id, bunch.Slug, cashgame.LocationId, locationItems, selectedEventId, eventItems);
        }

        public class Request
        {
            public string UserName { get; }
            public int Id { get; }

            public Request(string userName, int id)
            {
                UserName = userName;
                Id = id;
            }
        }

        public class Result
        {
            public string Date { get; private set; }
            public int CashgameId { get; private set; }
            public string Slug { get; private set; }
            public int LocationId { get; private set; }
            public IList<LocationItem> Locations { get; private set; }
            public int SelectedEventId { get; private set; }
            public IList<EventItem> Events { get; private set; }

            public Result(string date, int cashgameId, string slug, int locationId, IList<LocationItem> locations, int selectedEventId, IList<EventItem> events)
            {
                Date = date;
                CashgameId = cashgameId;
                Slug = slug;
                LocationId = locationId;
                Locations = locations;
                SelectedEventId = selectedEventId;
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