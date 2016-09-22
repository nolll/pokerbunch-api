using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class AddEvent
    {
        private readonly BunchService _bunchService;
        private readonly PlayerService _playerService;
        private readonly UserService _userService;
        private readonly EventService _eventService;

        public AddEvent(BunchService bunchService, PlayerService playerService, UserService userService, EventService eventService)
        {
            _bunchService = bunchService;
            _playerService = playerService;
            _userService = userService;
            _eventService = eventService;
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

            var e = new Event(0, bunch.Id, request.Name);
            _eventService.Add(e);

            return new Result(bunch.Slug);
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
            public string Slug { get; private set; }

            public Result(string slug)
            {
                Slug = slug;
            }
        }
    }
}