using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Checkpoints;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class Buyin(
    ICashgameRepository cashgameRepository,
    IPlayerRepository playerRepository,
    IUserRepository userRepository)
    : UseCase<Buyin.Request, Buyin.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var cashgame = await cashgameRepository.Get(request.CashgameId);
        var currentUser = await userRepository.GetByUserName(request.UserName);
        var currentPlayer = await playerRepository.Get(cashgame.BunchId, currentUser.Id);
        if (!AccessControl.CanEditCashgameActionsFor(request.PlayerId, currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var stackAfterBuyin = request.StackAmount + request.BuyinAmount;
        var checkpoint = new BuyinCheckpoint(cashgame.Id, request.PlayerId, request.CurrentTime, stackAfterBuyin, request.BuyinAmount, null);
        cashgame.AddCheckpoint(checkpoint);
        await cashgameRepository.Update(cashgame);

        return Success(new Result());
    }
    
    public class Request(
        string userName,
        string cashgameId,
        string playerId,
        int buyinAmount,
        int stackAmount,
        DateTime currentTime)
    {
        public string UserName { get; } = userName;
        public string CashgameId { get; } = cashgameId;
        public string PlayerId { get; } = playerId;

        [Range(1, int.MaxValue, ErrorMessage = "Amount needs to be positive")]
        public int BuyinAmount { get; } = buyinAmount;

        [Range(0, int.MaxValue, ErrorMessage = "Stack can't be negative")]
        public int StackAmount { get; } = stackAmount;

        public DateTime CurrentTime { get; } = currentTime;
    }

    public class Result
    {
    }
}