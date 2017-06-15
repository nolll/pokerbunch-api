using System;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class EventDetails
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IBunchRepository _bunchRepository;
        private readonly ILocationRepository _locationRepository;

        public EventDetails(IEventRepository eventRepository, IUserRepository userRepository, IPlayerRepository playerRepository, IBunchRepository bunchRepository, ILocationRepository locationRepository)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _playerRepository = playerRepository;
            _bunchRepository = bunchRepository;
            _locationRepository = locationRepository;
        }

        public Result Execute(Request request)
        {
            var e = _eventRepository.Get(request.EventId);
            var location = e.LocationId > 0 ? _locationRepository.Get(e.LocationId) : null;
            var bunch = _bunchRepository.Get(e.BunchId);
            var user = _userRepository.Get(request.UserName);
            var player = _playerRepository.Get(e.BunchId, user.Id);
            RequireRole.Player(user, player);

            var locationId = location?.Id ?? 0;
            var locationName = location?.Name;
            
            return new Result(e.Id, e.Name, bunch.Slug, locationId, locationName, e.StartDate);
        }

        public class Request
        {
            public string UserName { get; }
            public int EventId { get; }

            public Request(string userName, int eventId)
            {
                UserName = userName;
                EventId = eventId;
            }
        }

        public class Result
        {
            public int Id { get; }
            public string Name { get; }
            public string BunchId { get; }
            public int LocationId { get; }
            public string LocationName { get; }
            public Date StartDate { get; }

            public Result(int id, string name, string bunchId, int locationId, string locationName, Date startDate)
            {
                Id = id;
                Name = name;
                BunchId = bunchId;
                LocationId = locationId;
                LocationName = locationName;
                StartDate = startDate;
            }
        }
    }
}