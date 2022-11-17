﻿using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class DeleteCashgame : UseCase<DeleteCashgame.Request, DeleteCashgame.Result>
{
    private readonly ICashgameRepository _cashgameRepository;
    private readonly IBunchRepository _bunchRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;

    public DeleteCashgame(ICashgameRepository cashgameRepository, IBunchRepository bunchRepository, IUserRepository userRepository, IPlayerRepository playerRepository)
    {
        _cashgameRepository = cashgameRepository;
        _bunchRepository = bunchRepository;
        _userRepository = userRepository;
        _playerRepository = playerRepository;
    }

    protected override UseCaseResult<Result> Work(Request request)
    {
        var cashgame = _cashgameRepository.Get(request.Id);
        var bunch = _bunchRepository.Get(cashgame.BunchId);
        var currentUser = _userRepository.Get(request.UserName);
        var currentPlayer = _playerRepository.Get(bunch.Id, currentUser.Id);

        if (!AccessControl.CanDeleteCashgame(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        if (cashgame.PlayerCount > 0)
            return Error(new CashgameHasResultsError());

        _cashgameRepository.DeleteGame(cashgame.Id);

        return Success(new Result(bunch.Slug));
    }
    
    public class Request
    {
        public string UserName { get; }
        public int Id { get; }

        public Request(string userName, int id)
        {
            UserName = userName;
            Id = id;
        }
    }

    public class Result
    {
        public string Slug { get; private set; }

        public Result(string slug)
        {
            Slug = slug;
        }
    }
}