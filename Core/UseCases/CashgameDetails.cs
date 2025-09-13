using System;
using System.Linq;
using Core.Entities;
using Core.Entities.Checkpoints;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class CashgameDetails(
    IBunchRepository bunchRepository,
    ICashgameRepository cashgameRepository,
    IPlayerRepository playerRepository,
    ILocationRepository locationRepository,
    IEventRepository eventRepository)
    : UseCase<CashgameDetails.Request, CashgameDetails.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var cashgame = await cashgameRepository.Get(request.Id);
        var bunch = await bunchRepository.Get(cashgame.BunchId);
        var bunchAccess = request.Auth.GetBunchById(cashgame.BunchId);

        if (!request.Auth.CanSeeCashgame(cashgame.BunchId))
            return Error(new AccessDeniedError());

        var playerIds = GetPlayerIds(cashgame);
        var players = await playerRepository.Get(playerIds);

        var location = await locationRepository.Get(cashgame.LocationId);
        var @event = cashgame.EventId != null ? await eventRepository.Get(cashgame.EventId) : null;
        var eventName = @event?.Name;
        var eventId = @event?.Id;

        var playerItems = GetPlayerItems(cashgame, players);

        var defaultBuyin = bunch.DefaultBuyin;
        var currencyFormat = bunch.Currency.Format;
        var currencySymbol = bunch.Currency.Symbol;
        var currencyLayout = bunch.Currency.Layout;
        var thousandSeparator = bunch.Currency.ThousandSeparator;
        var culture = bunch.Currency.Culture.Name;
        
        var startTime = GetStartTime(playerItems, request.CurrentUtc);
        var updatedTime = GetUpdatedTime(playerItems, cashgame.Status, request.CurrentUtc);
        
        var result = new Result(
            bunch.Slug,
            bunch.Timezone.Id,
            bunchAccess.PlayerId,
            cashgame.Id,
            startTime,
            updatedTime,
            location.Name,
            location.Id,
            eventName,
            eventId,
            playerItems,
            defaultBuyin,
            currencyFormat,
            currencySymbol,
            currencyLayout,
            thousandSeparator,
            culture,
            bunchAccess.Role,
            cashgame.Status == GameStatus.Running);

        return Success(result);
    }
    
    private static DateTime GetStartTime(IList<RunningCashgamePlayerItem> playerItems, DateTime currentUtc) => 
        playerItems.Any() ? playerItems.Min(o => o.BuyinTime) : currentUtc;

    private static DateTime GetUpdatedTime(IList<RunningCashgamePlayerItem> playerItems, GameStatus status, DateTime currentUtc) => 
        status == GameStatus.Finished && playerItems.Any() ? playerItems.Max(o => o.UpdatedTime) : currentUtc;

    private static IList<string> GetPlayerIds(Cashgame cashgame) => 
        cashgame.Checkpoints.Select(o => o.PlayerId).Distinct().ToList();

    private static IList<RunningCashgamePlayerItem> GetPlayerItems(Cashgame cashgame, IList<Player> players)
    {
        var results = GetSortedResults(cashgame);
        var items = new List<RunningCashgamePlayerItem>();
        foreach (var result in results)
        {
            var playerId = result.PlayerId;
            var player = players.First(o => o.Id == playerId);
            var hasCheckedOut = result.CashoutCheckpoint != null;
            var item = new RunningCashgamePlayerItem(playerId, player.DisplayName, player.Color, cashgame.Id, hasCheckedOut, result.Checkpoints);
            items.Add(item);
        }

        return items;
    }

    private static IEnumerable<CashgameResult> GetSortedResults(Cashgame cashgame)
    {
        var results = cashgame.Results;
        return results.OrderByDescending(o => o.Winnings);
    }

    public class Request(IAuth auth, string id, DateTime currentUtc)
    {
        public IAuth Auth { get; } = auth;
        public string Id { get; } = id;
        public DateTime CurrentUtc { get; } = currentUtc;
    }

    public class Result(
        string slug,
        string timezone,
        string playerId,
        string cashgameId,
        DateTime startTime,
        DateTime updatedTime,
        string locationName,
        string locationId,
        string? eventName,
        string? eventId,
        IList<RunningCashgamePlayerItem> playerItems,
        int defaultBuyin,
        string currencyFormat,
        string currencySymbol,
        string currencyLayout,
        string thousandSeparator,
        string culture,
        Role role,
        bool isRunning)
    {
        public string Slug { get; } = slug;
        public string Timezone { get; } = timezone;
        public string PlayerId { get; } = playerId;
        public string CashgameId { get; } = cashgameId;
        public DateTime StartTime { get; } = startTime;
        public DateTime UpdatedTime { get; } = updatedTime;
        public string LocationName { get; } = locationName;
        public string LocationId { get; } = locationId;
        public string? EventName { get; } = eventName;
        public string? EventId { get; } = eventId;
        public IList<RunningCashgamePlayerItem> PlayerItems { get; } = playerItems;
        public int DefaultBuyin { get; } = defaultBuyin;
        public string CurrencyFormat { get; } = currencyFormat;
        public string CurrencySymbol { get; } = currencySymbol;
        public string CurrencyLayout { get; } = currencyLayout;
        public string ThousandSeparator { get; } = thousandSeparator;
        public string Culture { get; } = culture;
        public Role Role { get; } = role;
        public bool IsRunning { get; } = isRunning;
    }

    public class RunningCashgameActionItem(Checkpoint checkpoint)
    {
        public string Id { get; } = checkpoint.Id;
        public CheckpointType Type { get; } = checkpoint.Type;
        public DateTime Time { get; } = checkpoint.Timestamp;
        public int Stack { get; } = checkpoint.Stack;
        public int AddedMoney { get; } = checkpoint.Amount;
    }

    public class RunningCashgamePlayerItem
    {
        public string PlayerId { get; }
        public string Name { get; }
        public string? Color { get; }
        public string CashgameId { get; }
        public bool HasCashedOut { get; }
        public int Buyin { get; }
        public int Stack { get; }
        public DateTime BuyinTime { get; }
        public DateTime UpdatedTime { get; }
        public IList<RunningCashgameActionItem> Checkpoints { get; }

        public RunningCashgamePlayerItem(
            string playerId,
            string name,
            string? color,
            string cashgameId,
            bool hasCashedOut,
            IList<Checkpoint> checkpoints)
        {
            PlayerId = playerId;
            Name = name;
            Color = color;
            CashgameId = cashgameId;
            HasCashedOut = hasCashedOut;
            var list = checkpoints.ToList();
            var lastCheckpoint = list.Last();
            Checkpoints = list.Select(o => new RunningCashgameActionItem(o)).ToList();
            Buyin = list.Sum(o => o.Amount);
            Stack = lastCheckpoint.Stack;
            BuyinTime = checkpoints.First().Timestamp;
            UpdatedTime = lastCheckpoint.Timestamp;
        }
    }
}