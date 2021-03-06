using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class PlayerCashgameList
{
    private readonly IBunchRepository _bunchRepository;
    private readonly ICashgameRepository _cashgameRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ILocationRepository _locationRepository;

    public PlayerCashgameList(IBunchRepository bunchRepository, ICashgameRepository cashgameRepository, IUserRepository userRepository, IPlayerRepository playerRepository, ILocationRepository locationRepository)
    {
        _bunchRepository = bunchRepository;
        _cashgameRepository = cashgameRepository;
        _userRepository = userRepository;
        _playerRepository = playerRepository;
        _locationRepository = locationRepository;
    }

    public Result Execute(Request request)
    {
        var player = _playerRepository.Get(request.PlayerId);
        var bunch = _bunchRepository.Get(player.BunchId);
        var user = _userRepository.Get(request.UserName);
        RequireRole.Player(user, player);
        var cashgames = _cashgameRepository.GetByPlayer(request.PlayerId);
        cashgames = SortItems(cashgames).ToList();
        var locations = _locationRepository.List(bunch.Id);
        var players = _playerRepository.List(bunch.Id);
        var items = cashgames.Select(o => new Item(o, GetLocation(o, locations), players));

        return new Result(items.ToList());
    }

    private Location GetLocation(Cashgame cashgame, IEnumerable<Location> locations)
    {
        return locations.First(o => o.Id == cashgame.LocationId);
    }

    private static IEnumerable<Cashgame> SortItems(IEnumerable<Cashgame> items)
    {
        return items.OrderByDescending(o => o.StartTime);
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
        public IList<Item> Items { get; }

        public Result(IList<Item> items)
        {
            Items = items;
        }
    }

    public class Item
    {
        public int LocationId { get; }
        public string LocationName { get; }
        public int CashgameId { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public IList<ItemResult> Players { get; }

        public Item(Cashgame cashgame, Location location, IList<Player> players)
        {
            LocationId = location.Id;
            LocationName = location.Name;
            CashgameId = cashgame.Id;
            StartTime = cashgame.StartTime ?? DateTime.MinValue;
            EndTime = cashgame.EndTime ?? DateTime.MinValue;
            Players = cashgame.Results.Select(o => new ItemResult(o, GetPlayerName(o.PlayerId, players))).ToList();
        }
    }

    private static string GetPlayerName(int playerId, IList<Player> players)
    {
        var player = players.FirstOrDefault(o => o.Id == playerId);
        return player?.DisplayName ?? "";
    }

    public class ItemResult
    {
        public ItemPlayer Player { get; }
        public DateTime BuyinTime { get; }
        public DateTime UpdatedTime { get; }
        public int Buyin { get; }
        public int Stack { get; }

        public ItemResult(CashgameResult result, string playerName)
        {
            Player = new ItemPlayer(result.PlayerId, playerName);
            BuyinTime = result.BuyinTime ?? DateTime.MinValue;
            UpdatedTime = result.LastReportTime;
            Buyin = result.Buyin;
            Stack = result.Stack;
        }
    }

    public class ItemPlayer
    {
        public int Id { get; }
        public string Name { get; }

        public ItemPlayer(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}