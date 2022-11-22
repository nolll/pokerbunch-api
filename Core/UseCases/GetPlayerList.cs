using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetPlayerList : UseCase<GetPlayerList.Request, GetPlayerList.Result>
{
    private readonly IBunchRepository _bunchRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;

    public GetPlayerList(IBunchRepository bunchRepository, IUserRepository userRepository, IPlayerRepository playerRepository)
    {
        _bunchRepository = bunchRepository;
        _userRepository = userRepository;
        _playerRepository = playerRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var bunch = await _bunchRepository.GetBySlug(request.Slug);
        var currentUser = await _userRepository.GetByUserNameOrEmail(request.UserName);
        var currentPlayer = await _playerRepository.Get(bunch.Id, currentUser.Id);

        if (!AccessControl.CanListPlayers(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var players = await _playerRepository.List(bunch.Id);
        var canAddPlayer = AccessControl.CanAddPlayer(currentUser, currentPlayer);

        return Success(new Result(bunch, players, canAddPlayer));
    }

    public class Request
    {
        public string UserName { get; }
        public string Slug { get; }

        public Request(string userName, string slug)
        {
            UserName = userName;
            Slug = slug;
        }
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

    public class ResultItem
    {
        public string Name { get; }
        public string Id { get; }
        public string Color { get; }
        public string UserId { get; }
        public string UserName { get; }

        public ResultItem(Player player)
        {
            Name = player.DisplayName;
            Id = player.Id;
            Color = player.Color;
            UserId = player.IsUser ? player.UserId : null;
            UserName = player.IsUser ? player.UserName : null;
        }
    }
}