using System.Collections.Generic;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class CurrentCashgames
{
    private readonly IUserRepository _userRepository;
    private readonly IBunchRepository _bunchRepository;
    private readonly ICashgameRepository _cashgameRepository;
    private readonly IPlayerRepository _playerRepository;

    public CurrentCashgames(
        IUserRepository userRepository,
        IBunchRepository bunchRepository,
        ICashgameRepository cashgameRepository,
        IPlayerRepository playerRepository)
    {
        _userRepository = userRepository;
        _bunchRepository = bunchRepository;
        _cashgameRepository = cashgameRepository;
        _playerRepository = playerRepository;
    }

    public Result Execute(Request request)
    {
        var bunch = _bunchRepository.GetBySlug(request.Slug);
        var user = _userRepository.Get(request.UserName);
        var player = _playerRepository.Get(bunch.Id, user.Id);
        RequireRole.Player(user, player);

        var cashgame = _cashgameRepository.GetRunning(bunch.Id);

        var gameList = new List<Game>();
        if (cashgame != null)
            gameList.Add(new Game(bunch.Slug, cashgame.Id));
            
        return new Result(gameList);
    }

    public class Request
    {
        public string UserName { get; }
        public string Slug { get; }

        public Request(string userName, string slug)
        {
            UserName = userName;
            Slug = slug;
        }
    }

    public class Result
    {
        public List<Game> Games { get; }

        public Result(List<Game> games)
        {
            Games = games;
        }
    }

    public class Game
    {
        public string Slug { get; }
        public int Id { get; }

        public Game(string slug, int id)
        {
            Slug = slug;
            Id = id;
        }
    }
}