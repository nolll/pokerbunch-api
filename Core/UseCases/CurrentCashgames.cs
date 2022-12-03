using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class CurrentCashgames : UseCase<CurrentCashgames.Request, CurrentCashgames.Result>
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

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var bunch = await _bunchRepository.GetBySlug(request.Slug);
        var user = await _userRepository.GetByUserName(request.UserName);
        var player = await _playerRepository.Get(bunch.Id, user.Id);
        if (!AccessControl.CanListCurrentGames(user, player))
            return Error(new AccessDeniedError());

        var cashgame = await _cashgameRepository.GetRunning(bunch.Id);

        var gameList = new List<Game>();
        if (cashgame != null)
            gameList.Add(new Game(bunch.Slug, cashgame.Id));

        return Success(new Result(gameList));
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
        public string Id { get; }

        public Game(string slug, string id)
        {
            Slug = slug;
            Id = id;
        }
    }
}