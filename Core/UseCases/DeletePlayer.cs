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

        var bunchInfo = request.AccessControl.GetBunchById(player.BunchId);
        if (!request.AccessControl.CanDeletePlayer(player.BunchId))
            return Error(new AccessDeniedError());

        var cashgames = await cashgameRepository.GetByPlayer(player.Id);
        var hasPlayed = cashgames.Any();

        if (hasPlayed)
            return Error(new PlayerHasGamesError());

        await playerRepository.Delete(request.PlayerId);

        return Success(new Result(bunchInfo.Slug, request.PlayerId));
    }
    
    public class Request(IAccessControl accessControl, string playerId)
    {
        public IAccessControl AccessControl { get; } = accessControl;
        public string PlayerId { get; } = playerId;
    }

    public class Result(string slug, string playerId)
    {
        public string Slug { get; private set; } = slug;
        public string PlayerId { get; private set; } = playerId;
    }
}