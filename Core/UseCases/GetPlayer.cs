using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetPlayer
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

    public Result Execute(Request request)
    {
        var player = _playerRepository.Get(request.PlayerId);
        var bunch = _bunchRepository.Get(player.BunchId);
        var user = _userRepository.Get(player.UserId);
        var currentUser = _userRepository.Get(request.UserName);
        var currentPlayer = _playerRepository.Get(bunch.Id, currentUser.Id);
        RequireRole.Player(currentUser, currentPlayer);
        var isManager = RoleHandler.IsInRole(currentUser, currentPlayer, Role.Manager);
        var cashgames = _cashgameRepository.GetByPlayer(player.Id);
        var hasPlayed = cashgames.Any();
        var avatarUrl = user != null ? GravatarService.GetAvatarUrl(user.Email) : string.Empty;

        return new Result(bunch, player, user, isManager, hasPlayed, avatarUrl);
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

        public Result(Bunch bunch, Player player, User user, bool isManager, bool hasPlayed, string avatarUrl)
        {
            var isUser = user != null;

            DisplayName = player.DisplayName;
            PlayerId = player.Id;
            CanDelete = isManager && !hasPlayed;
            IsUser = isUser;
            UserId = user?.Id;
            UserName = isUser ? user.UserName : string.Empty;
            AvatarUrl = avatarUrl;
            Color = player.Color;
            Slug = bunch.Slug;
        }
    }
}