using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetPlayer : UseCase<GetPlayer.Request, GetPlayer.Result>
{
    private readonly IBunchRepository _bunchRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ICashgameRepository _cashgameRepository;
    private readonly IUserRepository _userRepository;

    public GetPlayer(IBunchRepository bunchRepository, IPlayerRepository playerRepository, ICashgameRepository cashgameRepository, IUserRepository userRepository)
    {
        _bunchRepository = bunchRepository;
        _playerRepository = playerRepository;
        _cashgameRepository = cashgameRepository;
        _userRepository = userRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var player = await _playerRepository.Get(request.PlayerId);
        if (player == null)
            return Error(new PlayerNotFoundError(request.PlayerId));

        var bunch = await _bunchRepository.Get(player.BunchId);
        var user = player.UserId != null 
            ? await _userRepository.GetById(player.UserId)
            : null;
        var currentUser = await _userRepository.GetByUserName(request.UserName);
        var currentPlayer = await _playerRepository.Get(bunch.Id, currentUser.Id);
        if (!AccessControl.CanSeePlayer(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var canDelete = AccessControl.CanDeletePlayer(currentUser, currentPlayer);
        var cashgames = await _cashgameRepository.GetByPlayer(player.Id);
        var hasPlayed = cashgames.Any();
        var avatarUrl = user != null ? GravatarService.GetAvatarUrl(user.Email) : string.Empty;

        return Success(new Result(bunch, player, user, canDelete, hasPlayed, avatarUrl));
    }

    public class Request
    {
        public string UserName { get; }
        public string PlayerId { get; }

        public Request(string userName, string playerId)
        {
            UserName = userName;
            PlayerId = playerId;
        }
    }

    public class Result
    {
        public string DisplayName { get; }
        public string PlayerId { get; }
        public bool CanDelete { get; }
        public bool IsUser { get; }
        public string UserId { get; }
        public string UserName { get; }
        public string AvatarUrl { get; }
        public string Slug { get; }
        public string Color { get; }

        public Result(Bunch bunch, Player player, User user, bool canDelete, bool hasPlayed, string avatarUrl)
        {
            var isUser = user != null;

            DisplayName = player.DisplayName;
            PlayerId = player.Id;
            CanDelete = canDelete && !hasPlayed;
            IsUser = isUser;
            UserId = user?.Id;
            UserName = isUser ? user.UserName : null;
            AvatarUrl = avatarUrl;
            Color = player.Color;
            Slug = bunch.Slug;
        }
    }
}