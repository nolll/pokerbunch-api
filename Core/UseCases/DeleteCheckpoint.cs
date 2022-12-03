﻿using Core.Entities;
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

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var cashgame = await _cashgameRepository.GetByCheckpoint(request.CheckpointId);
        var checkpoint = cashgame.GetCheckpoint(request.CheckpointId);
        var bunch = await _bunchRepository.Get(cashgame.BunchId);
        var currentUser = await _userRepository.GetByUserName(request.UserName);
        var currentPlayer = await _playerRepository.Get(cashgame.BunchId, currentUser.Id);

        if (!AccessControl.CanDeleteCheckpoint(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        cashgame.DeleteCheckpoint(checkpoint);
        await _cashgameRepository.Update(cashgame);

        var gameIsRunning = cashgame.Status == GameStatus.Running;
        return Success(new Result(bunch.Slug, gameIsRunning, cashgame.Id));
    }
    
    public class Request
    {
        public string UserName { get; }
        public string CheckpointId { get; }

        public Request(string userName, string checkpointId)
        {
            UserName = userName;
            CheckpointId = checkpointId;
        }
    }

    public class Result
    {
        public string Slug { get; private set; }
        public bool GameIsRunning { get; private set; }
        public string CashgameId { get; private set; }

        public Result(string slug, bool gameIsRunning, string cashgameId)
        {
            Slug = slug;
            GameIsRunning = gameIsRunning;
            CashgameId = cashgameId;
        }
    }
}