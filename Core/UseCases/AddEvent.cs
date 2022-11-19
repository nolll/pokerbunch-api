using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class AddEvent : UseCase<AddEvent.Request, AddEvent.Result>
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

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var bunch = await _bunchRepository.GetBySlug(request.Slug);
        var currentUser = await _userRepository.Get(request.UserName);
        var currentPlayer = await _playerRepository.Get(bunch.Id, currentUser.Id);

        if (!AccessControl.CanAddEvent(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var e = new Event(0, bunch.Id, request.Name);
        var id = await _eventRepository.Add(e);

        return Success(new Result(bunch.Slug, id));
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
        public string BunchId { get; }
        public int Id { get; }

        public Result(string bunchId, int id)
        {
            BunchId = bunchId;
            Id = id;
        }
    }
}