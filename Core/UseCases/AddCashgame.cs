using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class AddCashgame(
    IBunchRepository bunchRepository,
    ICashgameRepository cashgameRepository,
    ILocationRepository locationRepository)
    : UseCase<AddCashgame.Request, AddCashgame.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));
        
        var bunch = await bunchRepository.GetBySlug(request.Slug);

        var bunchInfo = request.Principal.GetBunchBySlug(request.Slug);
        if (!request.Principal.CanAddCashgame(bunchInfo.Id))
            return Error(new AccessDeniedError()); 

        var location = await locationRepository.Get(request.LocationId!);
        var cashgame = new Cashgame(bunchInfo.Id, location.Id, null, GameStatus.Running);
        var cashgameId = await cashgameRepository.Add(bunch, cashgame);

        return Success(new Result(request.Slug, cashgameId));
    }
    
    public class Request(IPrincipal principal, string slug, string? locationId)
    {
        public IPrincipal Principal { get; } = principal;
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