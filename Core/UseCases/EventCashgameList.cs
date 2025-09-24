using System;
using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class EventCashgameList(
    ICashgameRepository cashgameRepository,
    IPlayerRepository playerRepository,
    ILocationRepository locationRepository,
    IEventRepository eventRepository)
    : UseCase<EventCashgameList.Request, EventCashgameList.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var @event = await eventRepository.Get(request.EventId);

        if (!request.Auth.CanListEventCashgames(@event.BunchSlug))
            return Error(new AccessDeniedError());

        var cashgames = await cashgameRepository.GetByEvent(request.EventId);
        cashgames = SortItems(cashgames).ToList();
        var locations = await locationRepository.List(@event.BunchSlug);
        var players = await playerRepository.List(@event.BunchId);
        var items = cashgames.Select(o => new Item(o, GetLocation(o, locations), players));

        return Success(new Result(items.ToList()));
    }

    private static Location GetLocation(Cashgame cashgame, IEnumerable<Location> locations) => 
        locations.First(o => o.Id == cashgame.LocationId);

    private static IEnumerable<Cashgame> SortItems(IEnumerable<Cashgame> items) => 
        items.OrderByDescending(o => o.StartTime);

    public class Request(IAuth auth, string eventId)
    {
        public IAuth Auth { get; } = auth;
        public string EventId { get; } = eventId;
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
        public IList<ItemResult> Results { get; }

        public Item(Cashgame cashgame, Location location, IList<Player> players)
        {
            LocationId = location.Id;
            LocationName = location.Name;
            CashgameId = cashgame.Id;
            StartTime = cashgame.StartTime ?? DateTime.MinValue;
            EndTime = cashgame.EndTime ?? DateTime.MinValue;
            Results = cashgame.Results.Select(o => new ItemResult(o, GetPlayerName(o.PlayerId, players))).ToList();
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