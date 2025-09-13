using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetPlayerList(
    IBunchRepository bunchRepository,
    IPlayerRepository playerRepository)
    : UseCase<GetPlayerList.Request, GetPlayerList.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var bunch = await bunchRepository.GetBySlug(request.Slug);

        if (!request.Auth.CanListPlayers(bunch.Id))
            return Error(new AccessDeniedError());

        var players = await playerRepository.List(bunch.Id);
        var canAddPlayer = request.Auth.CanAddPlayer(bunch.Id);

        return Success(new Result(bunch, players, canAddPlayer));
    }

    public class Request(IAuth auth, string slug)
    {
        public IAuth Auth { get; } = auth;
        public string Slug { get; } = slug;
    }

    public class Result
    {
        public IList<ResultItem> Players { get; }
        public bool CanAddPlayer { get; }
        public string Slug { get; }

        public Result(Bunch bunch, IEnumerable<Player> players, bool canAddPlayer)
        {
            Players = players.Select(o => new ResultItem(o)).OrderBy(o => o.Name).ToList();
            CanAddPlayer = canAddPlayer;
            Slug = bunch.Slug;
        }
    }

    public class ResultItem(Player player)
    {
        public string Name { get; } = player.DisplayName;
        public string Id { get; } = player.Id;
        public string? Color { get; } = player.Color;
        public string? UserId { get; } = player.IsUser ? player.UserId : null;
        public string? UserName { get; } = player.IsUser ? player.UserName : null;
    }
}