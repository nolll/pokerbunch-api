using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class CurrentCashgames(
    IUserRepository userRepository,
    IBunchRepository bunchRepository,
    ICashgameRepository cashgameRepository,
    IPlayerRepository playerRepository)
    : UseCase<CurrentCashgames.Request, CurrentCashgames.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var bunch = await bunchRepository.GetBySlug(request.Slug);
        var user = await userRepository.GetByUserName(request.UserName);
        var player = await playerRepository.Get(bunch.Id, user.Id);
        if (!AccessControl.CanListCurrentGames(user, player))
            return Error(new AccessDeniedError());

        var cashgame = await cashgameRepository.GetRunning(bunch.Id);

        var gameList = new List<Game>();
        if (cashgame != null)
            gameList.Add(new Game(bunch.Slug, cashgame.Id));

        return Success(new Result(gameList));
    }
    
    public class Request(string userName, string slug)
    {
        public string UserName { get; } = userName;
        public string Slug { get; } = slug;
    }

    public class Result(List<Game> games)
    {
        public List<Game> Games { get; } = games;
    }

    public class Game(string slug, string id)
    {
        public string Slug { get; } = slug;
        public string Id { get; } = id;
    }
}