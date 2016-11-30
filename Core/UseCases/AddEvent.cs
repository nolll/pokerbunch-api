using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class AddEvent
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;

        public AddEvent(IBunchRepository bunchRepository, IPlayerRepository playerRepository, IUserRepository userRepository, IEventRepository eventRepository)
        {
            _bunchRepository = bunchRepository;
            _playerRepository = playerRepository;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
        }

        public Result Execute(Request request)
        {
            var validator = new Validator(request);

            if (!validator.IsValid)
                throw new ValidationException(validator);

            var bunch = _bunchRepository.GetBySlug(request.Slug);
            var currentUser = _userRepository.Get(request.UserName);
            var currentPlayer = _playerRepository.Get(bunch.Id, currentUser.Id);
            RequireRole.Player(currentUser, currentPlayer);

            var e = new Event(0, bunch.Id, request.Name);
            _eventRepository.Add(e);

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