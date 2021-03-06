using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases;

public class AddCashgame
{
    private readonly IBunchRepository _bunchRepository;
    private readonly ICashgameRepository _cashgameRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IEventRepository _eventRepository;

    public AddCashgame(IBunchRepository bunchRepository, ICashgameRepository cashgameRepository, IUserRepository userRepository, IPlayerRepository playerRepository, ILocationRepository locationRepository, IEventRepository eventRepository)
    {
        _bunchRepository = bunchRepository;
        _cashgameRepository = cashgameRepository;
        _userRepository = userRepository;
        _playerRepository = playerRepository;
        _locationRepository = locationRepository;
        _eventRepository = eventRepository;
    }

    public Result Execute(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            throw new ValidationException(validator);

        var user = _userRepository.Get(request.UserName);
        var bunch = _bunchRepository.GetBySlug(request.Slug);
        var player = _playerRepository.Get(bunch.Id, user.Id);
        RequireRole.Player(user, player);
        var location = _locationRepository.Get(request.LocationId);
        var cashgame = new Cashgame(bunch.Id, location.Id, 0, GameStatus.Running);
        var cashgameId = _cashgameRepository.Add(bunch, cashgame);

        return new Result(request.Slug, cashgameId);
    }

    public class Request
    {
        public string UserName { get; }
        public string Slug { get; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a location")]
        public int LocationId { get; }

        public Request(string userName, string slug, int locationId)
        {
            UserName = userName;
            Slug = slug;
            LocationId = locationId;
        }
    }

    public class Result
    {
        public string Slug { get; }
        public int CashgameId { get; }

        public Result(string slug, int cashgameId)
        {
            Slug = slug;
            CashgameId = cashgameId;
        }
    }
}