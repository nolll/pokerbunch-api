using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class DeleteCashgame(
    ICashgameRepository cashgameRepository,
    IBunchRepository bunchRepository,
    IUserRepository userRepository,
    IPlayerRepository playerRepository)
    : UseCase<DeleteCashgame.Request, DeleteCashgame.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var cashgame = await cashgameRepository.Get(request.Id);
        var bunch = await bunchRepository.Get(cashgame.BunchId);
        var currentUser = await userRepository.GetByUserName(request.UserName);
        var currentPlayer = await playerRepository.Get(bunch.Id, currentUser.Id);

        if (!AccessControl.CanDeleteCashgame(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        if (cashgame.EventId != null)
            return Error(new CashgameIsPartOfEventError());

        if (cashgame.PlayerCount > 0)
            return Error(new CashgameHasResultsError());

        await cashgameRepository.DeleteGame(cashgame.Id);

        return Success(new Result(bunch.Slug));
    }
    
    public class Request(string userName, string id)
    {
        public string UserName { get; } = userName;
        public string Id { get; } = id;
    }

    public class Result(string slug)
    {
        public string Slug { get; private set; } = slug;
    }
}