using System.Collections.Generic;
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

    protected override UseCaseResult<Result> Work(Request request)
    {
        var bunch = _bunchRepository.GetBySlug(request.Slug);
        var user = _userRepository.Get(request.UserName);
        var player = _playerRepository.Get(bunch.Id, user.Id);

        if (!AccessControl.CanListPlayers(user, player))
            return Error(new AccessDeniedError());

        var players = _playerRepository.List(bunch.Id);
        var isManager = RoleHandler.IsInRole(user, player, Role.Manager);

        return Success(new Result(bunch, players, isManager));
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

        public Result(Bunch bunch, IEnumerable<Player> players, bool isManager)
        {
            Players = players.Select(o => new ResultItem(o)).OrderBy(o => o.Name).ToList();
            CanAddPlayer = isManager;
            Slug = bunch.Slug;
        }
    }

    public class ResultItem
    {
        public string Name { get; }
        public int Id { get; }
        public string Color { get; }
        public string UserId { get; }
        public string UserName { get; }

        public ResultItem(Player player)
        {
            Name = player.DisplayName;
            Id = player.Id;
            Color = player.Color;
            UserId = player.IsUser ? player.UserId.ToString() : null;
            UserName = player.IsUser ? player.UserName : null;
        }
    }
}