using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases;

public class EditCashgame
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

    public void Execute(Request request)
    {
        var validator = new Validator(request);
        if(!validator.IsValid)
            throw new ValidationException(validator);

        var cashgame = _cashgameRepository.Get(request.Id);
        var user = _userRepository.Get(request.UserName);
        var player = _playerRepository.Get(cashgame.BunchId, user.Id);
        RequireRole.Manager(user, player);
        var location = _locationRepository.Get(request.LocationId);
        var @event = request.EventId != 0 ? _eventRepository.Get(request.EventId) : null;
        var eventId = @event?.Id ?? 0;
        cashgame = new Cashgame(cashgame.BunchId, location.Id, eventId, cashgame.Status, cashgame.Id);
        _cashgameRepository.Update(cashgame);

        if (request.EventId > 0)
        {
            _eventRepository.AddCashgame(request.EventId, cashgame.Id);
        }
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
}