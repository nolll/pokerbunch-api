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

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var cashgame = await _cashgameRepository.GetByCheckpoint(request.CheckpointId);
        var existingCheckpoint = cashgame.GetCheckpoint(request.CheckpointId);
        var bunch = await _bunchRepository.Get(cashgame.BunchId);
        var currentUser = await _userRepository.GetByUserNameOrEmail(request.UserName);
        var currentPlayer = await _playerRepository.Get(bunch.Id, currentUser.Id);

        if (!AccessControl.CanEditCheckpoint(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var postedCheckpoint = Checkpoint.Create(
            existingCheckpoint.CashgameId,
            existingCheckpoint.PlayerId,
            request.Timestamp.UtcDateTime,
            existingCheckpoint.Type,
            request.Stack,
            request.Amount,
            existingCheckpoint.Id);

        cashgame.UpdateCheckpoint(postedCheckpoint);
        await _cashgameRepository.Update(cashgame);

        return Success(new Result(cashgame.Id, existingCheckpoint.PlayerId));
    }
    
    public class Request
    {
        public string UserName { get; }
        public string CheckpointId { get; }
        public DateTimeOffset Timestamp { get; }
        [Range(0, int.MaxValue, ErrorMessage = "Stack can't be negative")]
        public int Stack { get; }
        [Range(0, int.MaxValue, ErrorMessage = "Amount can't be negative")]
        public int Amount { get; }

        public Request(string userName, string checkpointId, DateTimeOffset timestamp, int stack, int? amount)
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
        public string CashgameId { get; }
        public string PlayerId { get; }

        public Result(string cashgameId, string playerId)
        {
            CashgameId = cashgameId;
            PlayerId = playerId;
        }
    }
}