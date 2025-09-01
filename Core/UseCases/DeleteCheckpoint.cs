using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class DeleteCheckpoint(ICashgameRepository cashgameRepository)
    : UseCase<DeleteCheckpoint.Request, DeleteCheckpoint.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var cashgame = await cashgameRepository.GetByCheckpoint(request.CheckpointId);
        var checkpoint = cashgame.GetCheckpoint(request.CheckpointId);
        var bunchInfo = request.Principal.GetBunchById(cashgame.BunchId);

        if (!request.Principal.CanDeleteCheckpoint(cashgame.BunchId))
            return Error(new AccessDeniedError());

        cashgame.DeleteCheckpoint(checkpoint);
        await cashgameRepository.Update(cashgame);

        var gameIsRunning = cashgame.Status == GameStatus.Running;
        return Success(new Result(bunchInfo.Slug, gameIsRunning, cashgame.Id));
    }
    
    public class Request(IPrincipal principal, string checkpointId)
    {
        public IPrincipal Principal { get; } = principal;
        public string CheckpointId { get; } = checkpointId;
    }

    public class Result(string slug, bool gameIsRunning, string cashgameId)
    {
        public string Slug { get; private set; } = slug;
        public bool GameIsRunning { get; private set; } = gameIsRunning;
        public string CashgameId { get; private set; } = cashgameId;
    }
}