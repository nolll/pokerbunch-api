using System;
using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class PlayerCashgameList(
    ICashgameRepository cashgameRepository,
    IPlayerRepository playerRepository,
    ILocationRepository locationRepository)
    : UseCase<PlayerCashgameList.Request, PlayerCashgameList.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var player = await playerRepository.Get(request.PlayerId);

        if (!request.Auth.CanListPlayerCashgames(player.BunchSlug))
            return Error(new AccessDeniedError());

        var cashgames = await cashgameRepository.GetByPlayer(request.PlayerId);
        cashgames = SortItems(cashgames).ToList();
        var locations = await locationRepository.List(player.BunchSlug);
        var players = await playerRepository.List(player.BunchId);
        var items = cashgames.Select(o => new Item(o, GetLocation(o, locations), players));

        return Success(new Result(items.ToList()));
    }
    
    private static Location GetLocation(Cashgame cashgame, IEnumerable<Location> locations) => 
        locations.First(o => o.Id == cashgame.LocationId);

    private static IEnumerable<Cashgame> SortItems(IEnumerable<Cashgame> items) => 
        items.OrderByDescending(o => o.StartTime);

    public class Request(IAuth auth, string playerId)
    {
        public IAuth Auth { get; } = auth;
        public string PlayerId { get; } = playerId;
    }

    public class Result(IList<Item> items)
    {
        public IList<Item> Items { get; } = items;
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

    public class ItemResult(CashgameResult result, string playerName)
    {
        public ItemPlayer Player { get; } = new(result.PlayerId, playerName);
        public DateTime BuyinTime { get; } = result.BuyinTime ?? DateTime.MinValue;
        public DateTime UpdatedTime { get; } = result.LastReportTime;
        public int Buyin { get; } = result.Buyin;
        public int Stack { get; } = result.Stack;
    }

    public class ItemPlayer(string id, string name)
    {
        public string Id { get; } = id;
        public string Name { get; } = name;
    }
}