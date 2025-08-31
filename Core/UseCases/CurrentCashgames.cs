using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class CurrentCashgames(ICashgameRepository cashgameRepository)
    : UseCase<CurrentCashgames.Request, CurrentCashgames.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var bunchInfo = request.AccessControl.GetBunchBySlug(request.Slug);
        if (!request.AccessControl.CanListCurrentGames(bunchInfo.Id))
            return Error(new AccessDeniedError());

        var cashgame = await cashgameRepository.GetRunning(bunchInfo.Id);

        var gameList = new List<Game>();
        if (cashgame != null)
            gameList.Add(new Game(bunchInfo.Slug, cashgame.Id));

        return Success(new Result(gameList));
    }
    
    public class Request(IAccessControl accessControl, string slug)
    {
        public IAccessControl AccessControl { get; } = accessControl;
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