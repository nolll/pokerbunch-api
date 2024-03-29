﻿using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetPlayerList(
    IBunchRepository bunchRepository,
    IUserRepository userRepository,
    IPlayerRepository playerRepository)
    : UseCase<GetPlayerList.Request, GetPlayerList.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var bunch = await bunchRepository.GetBySlug(request.Slug);
        var currentUser = await userRepository.GetByUserName(request.UserName);
        var currentPlayer = await playerRepository.Get(bunch.Id, currentUser.Id);

        if (!AccessControl.CanListPlayers(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var players = await playerRepository.List(bunch.Id);
        var canAddPlayer = AccessControl.CanAddPlayer(currentUser, currentPlayer);

        return Success(new Result(bunch, players, canAddPlayer));
    }

    public class Request(string userName, string slug)
    {
        public string UserName { get; } = userName;
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