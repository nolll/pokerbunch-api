using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class AddCashgame(
    IBunchRepository bunchRepository,
    ICashgameRepository cashgameRepository,
    IUserRepository userRepository,
    IPlayerRepository playerRepository,
    ILocationRepository locationRepository)
    : UseCase<AddCashgame.Request, AddCashgame.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var user = await userRepository.GetByUserName(request.UserName);
        var bunch = await bunchRepository.GetBySlug(request.Slug);
        var player = await playerRepository.Get(bunch.Id, user.Id);

        if (!AccessControl.CanAddCashgame(user, player))
            return Error(new AccessDeniedError()); 

        var location = await locationRepository.Get(request.LocationId!);
        var cashgame = new Cashgame(bunch.Id, location.Id, null, GameStatus.Running);
        var cashgameId = await cashgameRepository.Add(bunch, cashgame);

        return Success(new Result(request.Slug, cashgameId));
    }
    
    public class Request(string userName, string slug, string? locationId)
    {
        public string UserName { get; } = userName;
        public string Slug { get; } = slug;

        [Required(ErrorMessage = "Please select a location")]
        public string? LocationId { get; } = locationId;
    }

    public class Result(string slug, string cashgameId)
    {
        public string Slug { get; } = slug;
        public string CashgameId { get; } = cashgameId;
    }
}