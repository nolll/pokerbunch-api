using System.Collections.Generic;
using System.Linq;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class GetLocationList
    {
        private readonly BunchService _bunchService;
        private readonly IUserRepository _userRepository;
        private readonly PlayerService _playerService;
        private readonly ILocationRepository _locationRepository;

        public GetLocationList(BunchService bunchService, IUserRepository userRepository, PlayerService playerService, ILocationRepository locationRepository)
        {
            _bunchService = bunchService;
            _userRepository = userRepository;
            _playerService = playerService;
            _locationRepository = locationRepository;
        }

        public Result Execute(Request request)
        {
            var bunch = _bunchService.GetBySlug(request.Slug);
            var user = _userRepository.Get(request.UserName);
            var player = _playerService.GetByUserId(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var locations = _locationRepository.List(bunch.Id);

            var locationItems = locations.Select(o => CreateLocationItem(o, bunch.Slug)).OrderBy(o => o.Name).ToList();

            return new Result(locationItems);
        }

        private static Location CreateLocationItem(Entities.Location location, string slug)
        {
            return new Location(location.Id, location.Name, slug);
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
            public IList<Location> Locations { get; }

            public Result(IList<Location> locations)
            {
                Locations = locations;
            }
        }

        public class Location
        {
            public int Id { get; }
            public string Name { get; }
            public string Slug { get; }

            public Location(int id, string name, string slug)
            {
                Id = id;
                Name = name;
                Slug = slug;
            }
        }
    }
}