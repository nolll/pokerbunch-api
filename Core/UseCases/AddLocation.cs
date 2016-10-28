using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class AddLocation
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly PlayerService _playerService;
        private readonly IUserRepository _userRepository;
        private readonly ILocationRepository _locationRepository;

        public AddLocation(IBunchRepository bunchRepository, PlayerService playerService, IUserRepository userRepository, ILocationRepository locationRepository)
        {
            _bunchRepository = bunchRepository;
            _playerService = playerService;
            _userRepository = userRepository;
            _locationRepository = locationRepository;
        }

        public Result Execute(Request request)
        {
            var validator = new Validator(request);

            if (!validator.IsValid)
                throw new ValidationException(validator);

            var bunch = _bunchRepository.GetBySlug(request.Slug);
            var currentUser = _userRepository.Get(request.UserName);
            var currentPlayer = _playerService.Get(bunch.Id, currentUser.Id);
            RequireRole.Player(currentUser, currentPlayer);

            var location = new Location(0, request.Name, bunch.Id);
            var id = _locationRepository.Add(location);

            return new Result(bunch.Slug, id, location.Name);
        }

        public class Request
        {
            public string UserName { get; }
            public string Slug { get; }
            [Required(ErrorMessage = "Name can't be empty")]
            public string Name { get; }

            public Request(string userName, string slug, string name)
            {
                UserName = userName;
                Slug = slug;
                Name = name;
            }
        }

        public class Result
        {
            public string Slug { get; }
            public int Id { get; }
            public string Name { get; }

            public Result(string slug, int id, string name)
            {
                Slug = slug;
                Id = id;
                Name = name;
            }
        }
    }
}