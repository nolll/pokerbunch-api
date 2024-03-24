using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class DeleteCheckpoint(
    IBunchRepository bunchRepository,
    ICashgameRepository cashgameRepository,
    IUserRepository userRepository,
    IPlayerRepository playerRepository)
    : UseCase<DeleteCheckpoint.Request, DeleteCheckpoint.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var cashgame = await cashgameRepository.GetByCheckpoint(request.CheckpointId);
        var checkpoint = cashgame.GetCheckpoint(request.CheckpointId);
        var bunch = await bunchRepository.Get(cashgame.BunchId);
        var currentUser = await userRepository.GetByUserName(request.UserName);
        var currentPlayer = await playerRepository.Get(cashgame.BunchId, currentUser.Id);

        if (!AccessControl.CanDeleteCheckpoint(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        cashgame.DeleteCheckpoint(checkpoint);
        await cashgameRepository.Update(cashgame);

        var gameIsRunning = cashgame.Status == GameStatus.Running;
        return Success(new Result(bunch.Slug, gameIsRunning, cashgame.Id));
    }
    
    public class Request(string userName, string checkpointId)
    {
        public string UserName { get; } = userName;
        public string CheckpointId { get; } = checkpointId;
    }

    public class Result(string slug, bool gameIsRunning, string cashgameId)
    {
        public string Slug { get; private set; } = slug;
        public bool GameIsRunning { get; private set; } = gameIsRunning;
        public string CashgameId { get; private set; } = cashgameId;
    }
}