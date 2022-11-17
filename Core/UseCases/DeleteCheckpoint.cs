using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class DeleteCheckpoint : UseCase<DeleteCheckpoint.Request, DeleteCheckpoint.Result>
{
    private readonly IBunchRepository _bunchRepository;
    private readonly ICashgameRepository _cashgameRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;

    public DeleteCheckpoint(IBunchRepository bunchRepository, ICashgameRepository cashgameRepository, IUserRepository userRepository, IPlayerRepository playerRepository)
    {
        _bunchRepository = bunchRepository;
        _cashgameRepository = cashgameRepository;
        _userRepository = userRepository;
        _playerRepository = playerRepository;
    }

    protected override UseCaseResult<Result> Work(Request request)
    {
        var cashgame = _cashgameRepository.GetByCheckpoint(request.CheckpointId);
        var checkpoint = cashgame.GetCheckpoint(request.CheckpointId);
        var bunch = _bunchRepository.Get(cashgame.BunchId);
        var currentUser = _userRepository.Get(request.UserName);
        var currentPlayer = _playerRepository.Get(cashgame.BunchId, currentUser.Id);

        if (!AccessControl.CanDeleteCheckpoint(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        cashgame.DeleteCheckpoint(checkpoint);
        _cashgameRepository.Update(cashgame);

        var gameIsRunning = cashgame.Status == GameStatus.Running;
        return Success(new Result(bunch.Slug, gameIsRunning, cashgame.Id));
    }
    
    public class Request
    {
        public string UserName { get; }
        public int CheckpointId { get; }

        public Request(string userName, int checkpointId)
        {
            UserName = userName;
            CheckpointId = checkpointId;
        }
    }

    public class Result
    {
        public string Slug { get; private set; }
        public bool GameIsRunning { get; private set; }
        public int CashgameId { get; private set; }

        public Result(string slug, bool gameIsRunning, int cashgameId)
        {
            Slug = slug;
            GameIsRunning = gameIsRunning;
            CashgameId = cashgameId;
        }
    }
}