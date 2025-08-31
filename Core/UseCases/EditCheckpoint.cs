using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Checkpoints;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class EditCheckpoint(ICashgameRepository cashgameRepository)
    : UseCase<EditCheckpoint.Request, EditCheckpoint.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var cashgame = await cashgameRepository.GetByCheckpoint(request.CheckpointId);
        var existingCheckpoint = cashgame.GetCheckpoint(request.CheckpointId);

        if (!request.Principal.CanEditCashgameAction(cashgame.BunchId))
            return Error(new AccessDeniedError());

        var postedCheckpoint = Checkpoint.Create(
            existingCheckpoint.Id,
            existingCheckpoint.CashgameId,
            existingCheckpoint.PlayerId,
            request.Timestamp,
            existingCheckpoint.Type,
            request.Stack,
            request.Amount);

        cashgame.UpdateCheckpoint(postedCheckpoint);
        await cashgameRepository.Update(cashgame);

        return Success(new Result(cashgame.Id, existingCheckpoint.PlayerId));
    }
    
    public class Request(
        IPrincipal principal,
        string checkpointId,
        DateTime timestamp,
        int stack,
        int? amount)
    {
        public IPrincipal Principal { get; } = principal;
        public string CheckpointId { get; } = checkpointId;
        public DateTime Timestamp { get; } = timestamp;

        [Range(0, int.MaxValue, ErrorMessage = "Stack can't be negative")]
        public int Stack { get; } = stack;

        [Range(0, int.MaxValue, ErrorMessage = "Amount can't be negative")]
        public int Amount { get; } = amount ?? 0;
    }

    public class Result(string cashgameId, string playerId)
    {
        public string CashgameId { get; } = cashgameId;
        public string PlayerId { get; } = playerId;
    }
}