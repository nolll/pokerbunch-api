using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Checkpoints;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class Report : UseCase<Report.Request, Report.Result>
{
    private readonly ICashgameRepository _cashgameRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IUserRepository _userRepository;

    public Report(ICashgameRepository cashgameRepository, IPlayerRepository playerRepository, IUserRepository userRepository)
    {
        _cashgameRepository = cashgameRepository;
        _playerRepository = playerRepository;
        _userRepository = userRepository;
    }

    protected override UseCaseResult<Result> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var cashgame = _cashgameRepository.Get(request.CashgameId);
        var currentUser = _userRepository.Get(request.UserName);
        var currentPlayer = _playerRepository.Get(cashgame.BunchId, currentUser.Id);
        if (!AccessControl.CanEditCashgameActionsFor(request.PlayerId, currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var checkpoint = Checkpoint.Create(cashgame.Id, request.PlayerId, request.CurrentTime, CheckpointType.Report, request.Stack);
        cashgame.AddCheckpoint(checkpoint);
        _cashgameRepository.Update(cashgame);

        return Success(new Result());
    }
    
    public class Request
    {
        public string UserName { get; }
        public int CashgameId { get; }
        public int PlayerId { get; }
        [Range(0, int.MaxValue, ErrorMessage = "Stack can't be negative")]
        public int Stack { get; }
        public DateTime CurrentTime { get; }

        public Request(string userName, int cashgameId, int playerId, int stack, DateTime currentTime)
        {
            UserName = userName;
            CashgameId = cashgameId;
            PlayerId = playerId;
            Stack = stack;
            CurrentTime = currentTime;
        }
    }

    public class Result
    {
    }
}