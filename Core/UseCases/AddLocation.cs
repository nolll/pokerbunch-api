using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class AddLocation
    {
        private readonly BunchService _bunchService;
        private readonly PlayerService _playerService;
        private readonly UserService _userService;
        private readonly LocationService _locationService;

        public AddLocation(BunchService bunchService, PlayerService playerService, UserService userService, LocationService locationService)
        {
            _bunchService = bunchService;
            _playerService = playerService;
            _userService = userService;
            _locationService = locationService;
        }

        public Result Execute(Request request)
        {
            var validator = new Validator(request);

            if (!validator.IsValid)
                throw new ValidationException(validator);

            var bunch = _bunchService.GetBySlug(request.Slug);
            var currentUser = _userService.GetByNameOrEmail(request.UserName);
            var currentPlayer = _playerService.GetByUserId(bunch.Id, currentUser.Id);
            RequireRole.Player(currentUser, currentPlayer);

            var location = new Location(0, request.Name, bunch.Id);
            var id = _locationService.Add(location);

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