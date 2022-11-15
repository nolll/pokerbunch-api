using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Checkpoints;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class EditCheckpoint : UseCase<EditCheckpoint.Request, EditCheckpoint.Result>
{
    private readonly IBunchRepository _bunchRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ICashgameRepository _cashgameRepository;

    public EditCheckpoint(IBunchRepository bunchRepository, IUserRepository userRepository, IPlayerRepository playerRepository, ICashgameRepository cashgameRepository)
    {
        _bunchRepository = bunchRepository;
        _userRepository = userRepository;
        _playerRepository = playerRepository;
        _cashgameRepository = cashgameRepository;
    }

    protected override UseCaseResult<Result> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var cashgame = _cashgameRepository.GetByCheckpoint(request.CheckpointId);
        var existingCheckpoint = cashgame.GetCheckpoint(request.CheckpointId);
        var bunch = _bunchRepository.Get(cashgame.BunchId);
        var currentUser = _userRepository.Get(request.UserName);
        var currentPlayer = _playerRepository.Get(bunch.Id, currentUser.Id);
        RequireRole.Manager(currentUser, currentPlayer);

        var postedCheckpoint = Checkpoint.Create(
            existingCheckpoint.CashgameId,
            existingCheckpoint.PlayerId,
            request.Timestamp.UtcDateTime,
            existingCheckpoint.Type,
            request.Stack,
            request.Amount,
            existingCheckpoint.Id);

        cashgame.UpdateCheckpoint(postedCheckpoint);
        _cashgameRepository.Update(cashgame);

        return Success(new Result(cashgame.Id, existingCheckpoint.PlayerId));
    }
    
    public class Request
    {
        public string UserName { get; }
        public int CheckpointId { get; }
        public DateTimeOffset Timestamp { get; }
        [Range(0, int.MaxValue, ErrorMessage = "Stack can't be negative")]
        public int Stack { get; }
        [Range(0, int.MaxValue, ErrorMessage = "Amount can't be negative")]
        public int Amount { get; }

        public Request(string userName, int checkpointId, DateTimeOffset timestamp, int stack, int? amount)
        {
            UserName = userName;
            CheckpointId = checkpointId;
            Timestamp = timestamp;
            Stack = stack;
            Amount = amount ?? 0;
        }
    }

    public class Result
    {
        public int CashgameId { get; }
        public int PlayerId { get; }

        public Result(int cashgameId, int playerId)
        {
            CashgameId = cashgameId;
            PlayerId = playerId;
        }
    }
}