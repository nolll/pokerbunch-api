using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Checkpoints;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class Report(ICashgameRepository cashgameRepository)
    : UseCase<Report.Request, Report.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var cashgame = await cashgameRepository.Get(request.CashgameId);
        if (!request.Principal.CanEditCashgameActionsFor(cashgame.BunchId, request.PlayerId))
            return Error(new AccessDeniedError());

        var checkpoint = Checkpoint.Create(
            "",
            cashgame.Id, 
            request.PlayerId, 
            request.CurrentTime, 
            CheckpointType.Report, 
            request.Stack);
        cashgame.AddCheckpoint(checkpoint);
        await cashgameRepository.Update(cashgame);

        return Success(new Result());
    }
    
    public class Request(
        IPrincipal principal,
        string cashgameId,
        string playerId,
        int stack,
        DateTime currentTime)
    {
        public IPrincipal Principal { get; } = principal;
        public string CashgameId { get; } = cashgameId;
        public string PlayerId { get; } = playerId;

        [Range(0, int.MaxValue, ErrorMessage = "Stack can't be negative")]
        public int Stack { get; } = stack;

        public DateTime CurrentTime { get; } = currentTime;
    }

    public class Result
    {
    }
}