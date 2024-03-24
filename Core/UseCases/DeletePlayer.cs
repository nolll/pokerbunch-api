using System.Linq;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class DeletePlayer(
    IPlayerRepository playerRepository,
    ICashgameRepository cashgameRepository,
    IUserRepository userRepository,
    IBunchRepository bunchRepository)
    : UseCase<DeletePlayer.Request, DeletePlayer.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var player = await playerRepository.Get(request.PlayerId);
        var bunch = await bunchRepository.Get(player.BunchId);
        var currentUser = await userRepository.GetByUserName(request.UserName);
        var currentPlayer = await playerRepository.Get(bunch.Id, currentUser.Id);

        if (!AccessControl.CanDeletePlayer(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var cashgames = await cashgameRepository.GetByPlayer(player.Id);
        var hasPlayed = cashgames.Any();

        if (hasPlayed)
            return Error(new PlayerHasGamesError());

        await playerRepository.Delete(request.PlayerId);

        return Success(new Result(bunch.Slug, request.PlayerId));
    }
    
    public class Request(string userName, string playerId)
    {
        public string UserName { get; } = userName;
        public string PlayerId { get; } = playerId;
    }

    public class Result(string slug, string playerId)
    {
        public string Slug { get; private set; } = slug;
        public string PlayerId { get; private set; } = playerId;
    }
}