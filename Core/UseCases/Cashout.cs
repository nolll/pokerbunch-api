﻿using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Entities.Checkpoints;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class Cashout(
    ICashgameRepository cashgameRepository,
    IPlayerRepository playerRepository,
    IUserRepository userRepository)
    : UseCase<Cashout.Request, Cashout.Result>
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

        var result = cashgame.GetResult(request.PlayerId);
        var existingCashoutCheckpoint = result?.CashoutCheckpoint;
        
        var postedCheckpoint = Checkpoint.Create(
            existingCashoutCheckpoint?.Id,
            cashgame.Id,
            request.PlayerId,
            request.CurrentTime,
            CheckpointType.Cashout,
            request.Stack);

        if (existingCashoutCheckpoint != null)
            cashgame.UpdateCheckpoint(postedCheckpoint);
        else
            cashgame.AddCheckpoint(postedCheckpoint);

        if (cashgame.IsReadyToEnd)
            cashgame.ChangeStatus(GameStatus.Finished);

        await cashgameRepository.Update(cashgame);

        return Success(new Result(cashgame.Id));
    }
    
    public class Request(string userName, string cashgameId, string playerId, int stack, DateTime currentTime)
    {
        public string UserName { get; } = userName;
        public string CashgameId { get; } = cashgameId;
        public string PlayerId { get; } = playerId;

        [Range(0, int.MaxValue, ErrorMessage = "Stack can't be negative")]
        public int Stack { get; } = stack;

        public DateTime CurrentTime { get; } = currentTime;
    }

    public class Result(string cashgameId)
    {
        public string CashgameId { get; } = cashgameId;
    }
}