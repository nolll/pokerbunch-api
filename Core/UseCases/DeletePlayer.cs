using System.Linq;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class DeletePlayer(
    IPlayerRepository playerRepository,
    ICashgameRepository cashgameRepository)
    : UseCase<DeletePlayer.Request, DeletePlayer.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var player = await playerRepository.Get(request.PlayerId);
        
        if (!request.Auth.CanDeletePlayer(player.BunchSlug))
            return Error(new AccessDeniedError());

        var cashgames = await cashgameRepository.GetByPlayer(player.Id);
        var hasPlayed = cashgames.Any();

        if (hasPlayed)
            return Error(new PlayerHasGamesError());

        await playerRepository.Delete(request.PlayerId);

        return Success(new Result());
    }
    
    public class Request(IAuth auth, string playerId)
    {
        public IAuth Auth { get; } = auth;
        public string PlayerId { get; } = playerId;
    }

    public class Result()
    {
    }
}