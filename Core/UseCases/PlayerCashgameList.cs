using System;
using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class PlayerCashgameList : UseCase<PlayerCashgameList.Request, PlayerCashgameList.Result>
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

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var player = await _playerRepository.Get(request.PlayerId);
        var bunch = await _bunchRepository.Get(player.BunchId);
        var currentUser = await _userRepository.GetByUserName(request.UserName);
        var currentPlayer = await _playerRepository.Get(bunch.Id, currentUser.Id);

        if (!AccessControl.CanListPlayerCashgames(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var cashgames = await _cashgameRepository.GetByPlayer(request.PlayerId);
        cashgames = SortItems(cashgames).ToList();
        var locations = await _locationRepository.List(bunch.Id);
        var players = await _playerRepository.List(bunch.Id);
        var items = cashgames.Select(o => new Item(o, GetLocation(o, locations), players));

        return Success(new Result(items.ToList()));
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
        public string PlayerId { get; }

        public Request(string userName, string playerId)
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
        public string LocationId { get; }
        public string LocationName { get; }
        public string CashgameId { get; }
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

    private static string GetPlayerName(string playerId, IList<Player> players)
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
        public string Id { get; }
        public string Name { get; }

        public ItemPlayer(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}