using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class AddCashgame(
    IBunchRepository bunchRepository,
    ICashgameRepository cashgameRepository)
    : UseCase<AddCashgame.Request, AddCashgame.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));
        
        var bunch = await bunchRepository.GetBySlug(request.Slug);
        
        if (!request.Auth.CanAddCashgame(bunch.Slug))
            return Error(new AccessDeniedError()); 
        
        var cashgame = new Cashgame(bunch.Id, bunch.Slug, request.LocationId!, null, GameStatus.Running);
        var cashgameId = await cashgameRepository.Add(bunch, cashgame);

        return Success(new Result(request.Slug, cashgameId));
    }
    
    public class Request(IAuth auth, string slug, string? locationId)
    {
        public IAuth Auth { get; } = auth;
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