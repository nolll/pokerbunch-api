using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class AddCashgame : UseCase<AddCashgame.Request, AddCashgame.Result>
{
    private readonly IBunchRepository _bunchRepository;
    private readonly ICashgameRepository _cashgameRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ILocationRepository _locationRepository;

    public AddCashgame(IBunchRepository bunchRepository, ICashgameRepository cashgameRepository, IUserRepository userRepository, IPlayerRepository playerRepository, ILocationRepository locationRepository)
    {
        _bunchRepository = bunchRepository;
        _cashgameRepository = cashgameRepository;
        _userRepository = userRepository;
        _playerRepository = playerRepository;
        _locationRepository = locationRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var user = await _userRepository.GetByUserNameOrEmail(request.UserName);
        var bunch = await _bunchRepository.GetBySlug(request.Slug);
        var player = await _playerRepository.Get(bunch.Id, user.Id);

        if (!AccessControl.CanAddCashgame(user, player))
            return Error(new AccessDeniedError()); 

        var location = await _locationRepository.Get(request.LocationId);
        var cashgame = new Cashgame(bunch.Id, location.Id, null, GameStatus.Running);
        var cashgameId = await _cashgameRepository.Add(bunch, cashgame);

        return Success(new Result(request.Slug, cashgameId));
    }
    
    public class Request
    {
        public string UserName { get; }
        public string Slug { get; }
        [Required(ErrorMessage = "Please select a location")]
        public string LocationId { get; }

        public Request(string userName, string slug, string locationId)
        {
            UserName = userName;
            Slug = slug;
            LocationId = locationId;
        }
    }

    public class Result
    {
        public string Slug { get; }
        public string CashgameId { get; }

        public Result(string slug, string cashgameId)
        {
            Slug = slug;
            CashgameId = cashgameId;
        }
    }
}