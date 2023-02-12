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

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var cashgame = await _cashgameRepository.Get(request.Id);
        var currentUser = await _userRepository.GetByUserName(request.UserName);
        var currentPlayer = await _playerRepository.Get(cashgame.BunchId, currentUser.Id);

        if (!AccessControl.CanEditCashgame(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var location = await _locationRepository.Get(request.LocationId!);
        var @event = request.EventId != null ? await _eventRepository.Get(request.EventId) : null;
        var eventId = @event?.Id;
        var updatedCashgame = new Cashgame(cashgame.BunchId, location.Id, eventId, cashgame.Status, cashgame.Id);
        await _cashgameRepository.Update(updatedCashgame);

        if (cashgame.EventId == null && request.EventId != null)
        {
            await _eventRepository.AddCashgame(request.EventId, updatedCashgame.Id);
        }
        else if(cashgame.EventId != null && request.EventId == null)
        {
            await _eventRepository.RemoveCashgame(cashgame.EventId, cashgame.Id);
        }

        return Success(new Result());
    }

    public class Request
    {
        public string UserName { get; }
        public string Id { get; }
        [Required(ErrorMessage = "Please select a location")]
        public string? LocationId { get; }
        public string? EventId { get; }

        public Request(string userName, string id, string? locationId, string? eventId)
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