using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetPlayer(
    IBunchRepository bunchRepository,
    IPlayerRepository playerRepository,
    ICashgameRepository cashgameRepository,
    IUserRepository userRepository)
    : UseCase<GetPlayer.Request, GetPlayer.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var player = await GetPlayerOrNull(request.PlayerId);
        if (player == null)
            return Error(new PlayerNotFoundError(request.PlayerId));
        
        var user = player.UserId != null 
            ? await userRepository.GetById(player.UserId)
            : null;
        
        if (!request.Auth.CanSeePlayer(player.BunchId))
            return Error(new AccessDeniedError());
        
        var bunch = await bunchRepository.Get(player.BunchId);

        var canDelete = request.Auth.CanDeletePlayer(player.BunchId);
        var cashgames = await cashgameRepository.GetByPlayer(player.Id);
        var hasPlayed = cashgames.Any();
        var avatarUrl = user != null ? GravatarService.GetAvatarUrl(user.Email) : "";

        return Success(new Result(bunch, player, user, canDelete, hasPlayed, avatarUrl));
    }

    private async Task<Player?> GetPlayerOrNull(string id)
    {
        try
        {
            return await playerRepository.Get(id);
        }
        catch
        {
            return null;
        }
    }

    public class Request(IAuth auth, string playerId)
    {
        public IAuth Auth { get; } = auth;
        public string PlayerId { get; } = playerId;
    }

    public class Result
    {
        public string DisplayName { get; }
        public string PlayerId { get; }
        public bool CanDelete { get; }
        public bool IsUser { get; }
        public string? UserId { get; }
        public string? UserName { get; }
        public string AvatarUrl { get; }
        public string Slug { get; }
        public string? Color { get; }

        public Result(Bunch bunch, Player player, User? user, bool canDelete, bool hasPlayed, string avatarUrl)
        {
            var isUser = user is not null;

            DisplayName = player.DisplayName;
            PlayerId = player.Id;
            CanDelete = canDelete && !hasPlayed;
            IsUser = isUser;
            UserId = user?.Id;
            UserName = user?.UserName ?? null;
            AvatarUrl = avatarUrl;
            Color = player.Color;
            Slug = bunch.Slug;
        }
    }
}