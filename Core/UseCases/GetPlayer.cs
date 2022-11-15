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

    protected override UseCaseResult<Result> Work(Request request)
    {
        var player = _playerRepository.Get(request.PlayerId);
        var bunch = _bunchRepository.Get(player.BunchId);
        var user = _userRepository.Get(player.UserId);
        var currentUser = _userRepository.Get(request.UserName);
        var currentPlayer = _playerRepository.Get(bunch.Id, currentUser.Id);
        if (!AccessControl.CanSeePlayer(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var canDelete = AccessControl.CanDeletePlayer(currentUser, currentPlayer);
        var cashgames = _cashgameRepository.GetByPlayer(player.Id);
        var hasPlayed = cashgames.Any();
        var avatarUrl = user != null ? GravatarService.GetAvatarUrl(user.Email) : string.Empty;

        return Success(new Result(bunch, player, user, canDelete, hasPlayed, avatarUrl));
    }

    public class Request
    {
        public string UserName { get; }
        public int PlayerId { get; }

        public Request(string userName, int playerId)
        {
            UserName = userName;
            PlayerId = playerId;
        }
    }

    public class Result
    {
        public string DisplayName { get; }
        public int PlayerId { get; }
        public bool CanDelete { get; }
        public bool IsUser { get; }
        public int? UserId { get; }
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
            UserName = isUser ? user.UserName : string.Empty;
            AvatarUrl = avatarUrl;
            Color = player.Color;
            Slug = bunch.Slug;
        }
    }
}