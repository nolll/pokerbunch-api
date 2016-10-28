using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class GetLocation
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IBunchRepository _bunchRepository;

        public GetLocation(ILocationRepository locationRepository, IUserRepository userRepository, IPlayerRepository playerRepository, IBunchRepository bunchRepository)
        {
            _locationRepository = locationRepository;
            _userRepository = userRepository;
            _playerRepository = playerRepository;
            _bunchRepository = bunchRepository;
        }

        public Result Execute(Request request)
        {
            var location = _locationRepository.Get(request.LocationId);
            var bunch = _bunchRepository.Get(location.BunchId);
            var user = _userRepository.Get(request.UserName);
            var player = _playerRepository.Get(location.BunchId, user.Id);
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