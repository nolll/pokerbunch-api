using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class GetLocation
    {
        private readonly ILocationRepository _locationRepository;
        private readonly UserService _userService;
        private readonly PlayerService _playerService;
        private readonly BunchService _bunchService;

        public GetLocation(ILocationRepository locationRepository, UserService userService, PlayerService playerService, BunchService bunchService)
        {
            _locationRepository = locationRepository;
            _userService = userService;
            _playerService = playerService;
            _bunchService = bunchService;
        }

        public Result Execute(Request request)
        {
            var location = _locationRepository.Get(request.LocationId);
            var bunch = _bunchService.Get(location.BunchId);
            var user = _userService.GetByNameOrEmail(request.UserName);
            var player = _playerService.GetByUserId(location.BunchId, user.Id);
            RequireRole.Player(user, player);

            return new Result(location.Id, location.Name, bunch.Slug);
        }

        public class Request
        {
            public string UserName { get; }
            public int LocationId { get; }

            public Request(string userName, int locationId)
            {
                UserName = userName;
                LocationId = locationId;
            }
        }

        public class Result
        {
            public int Id { get; private set; }
            public string Name { get; private set; }
            public string Slug { get; private set; }

            public Result(int id, string name, string slug)
            {
                Id = id;
                Name = name;
                Slug = slug;
            }
        }
    }
}