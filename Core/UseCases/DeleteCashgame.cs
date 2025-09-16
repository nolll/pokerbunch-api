using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class DeleteCashgame(ICashgameRepository cashgameRepository)
    : UseCase<DeleteCashgame.Request, DeleteCashgame.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var cashgame = await cashgameRepository.Get(request.Id);

        if (!request.Auth.CanDeleteCashgame(cashgame.BunchId))
            return Error(new AccessDeniedError());

        if (cashgame.EventId != null)
            return Error(new CashgameIsPartOfEventError());

        if (cashgame.PlayerCount > 0)
            return Error(new CashgameHasResultsError());

        await cashgameRepository.DeleteGame(cashgame.Id);

        return Success(new Result());
    }
    
    public class Request(IAuth auth, string id)
    {
        public IAuth Auth { get; } = auth;
        public string Id { get; } = id;
    }

    public class Result()
    {
    }
}