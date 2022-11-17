using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class EditCashgame : UseCase<EditCashgame.Request, EditCashgame.Result>
{
    private readonly ICashgameRepository _cashgameRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IEventRepository _eventRepository;

    public EditCashgame(ICashgameRepository cashgameRepository, IUserRepository userRepository, IPlayerRepository playerRepository, ILocationRepository locationRepository, IEventRepository eventRepository)
    {
        _cashgameRepository = cashgameRepository;
        _userRepository = userRepository;
        _playerRepository = playerRepository;
        _locationRepository = locationRepository;
        _eventRepository = eventRepository;
    }

    protected override UseCaseResult<Result> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var cashgame = _cashgameRepository.Get(request.Id);
        var currentUser = _userRepository.Get(request.UserName);
        var currentPlayer = _playerRepository.Get(cashgame.BunchId, currentUser.Id);

        if (!AccessControl.CanEditCashgame(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var location = _locationRepository.Get(request.LocationId);
        var @event = request.EventId != 0 ? _eventRepository.Get(request.EventId) : null;
        var eventId = @event?.Id ?? 0;
        cashgame = new Cashgame(cashgame.BunchId, location.Id, eventId, cashgame.Status, cashgame.Id);
        _cashgameRepository.Update(cashgame);

        if (request.EventId > 0)
        {
            _eventRepository.AddCashgame(request.EventId, cashgame.Id);
        }

        return Success(new Result());
    }

    public class Request
    {
        public string UserName { get; }
        public int Id { get; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a location")]
        public int LocationId { get; }
        public int EventId { get; }

        public Request(string userName, int id, int locationId, int eventId)
        {
            UserName = userName;
            Id = id;
            LocationId = locationId;
            EventId = eventId;
        }
    }

    public class Result
    {
    }
}