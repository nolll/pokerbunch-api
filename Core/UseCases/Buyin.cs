using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Checkpoints;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class Buyin : AsyncUseCase<Buyin.Request, Buyin.Result>
{
    private readonly ICashgameRepository _cashgameRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IUserRepository _userRepository;

    public Buyin(ICashgameRepository cashgameRepository, IPlayerRepository playerRepository, IUserRepository userRepository)
    {
        _cashgameRepository = cashgameRepository;
        _playerRepository = playerRepository;
        _userRepository = userRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var cashgame = _cashgameRepository.Get(request.CashgameId);
        var currentUser = await _userRepository.Get(request.UserName);
        var currentPlayer = _playerRepository.Get(cashgame.BunchId, currentUser.Id);
        if (!AccessControl.CanEditCashgameActionsFor(request.PlayerId, currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var stackAfterBuyin = request.StackAmount + request.BuyinAmount;
        var checkpoint = new BuyinCheckpoint(cashgame.Id, request.PlayerId, request.CurrentTime, stackAfterBuyin, request.BuyinAmount);
        cashgame.AddCheckpoint(checkpoint);
        _cashgameRepository.Update(cashgame);

        return Success(new Result());
    }
    
    public class Request
    {
        public string UserName { get; }
        public int CashgameId { get; }
        public int PlayerId { get; }
        [Range(1, int.MaxValue, ErrorMessage = "Amount needs to be positive")]
        public int BuyinAmount { get; }
        [Range(0, int.MaxValue, ErrorMessage = "Stack can't be negative")]
        public int StackAmount { get; }
        public DateTime CurrentTime { get; }

        public Request(string userName, int cashgameId, int playerId, int buyinAmount, int stackAmount, DateTime currentTime)
        {
            UserName = userName;
            CashgameId = cashgameId;
            PlayerId = playerId;
            BuyinAmount = buyinAmount;
            StackAmount = stackAmount;
            CurrentTime = currentTime;
        }
    }

    public class Result
    {
    }
}