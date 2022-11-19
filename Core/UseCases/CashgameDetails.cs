using System;
using System.Linq;
using Core.Entities;
using Core.Entities.Checkpoints;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class CashgameDetails : UseCase<CashgameDetails.Request, CashgameDetails.Result>
{
    private readonly IBunchRepository _bunchRepository;
    private readonly ICashgameRepository _cashgameRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IEventRepository _eventRepository;

    public CashgameDetails(IBunchRepository bunchRepository, ICashgameRepository cashgameRepository, IPlayerRepository playerRepository, IUserRepository userRepository, ILocationRepository locationRepository, IEventRepository eventRepository)
    {
        _bunchRepository = bunchRepository;
        _cashgameRepository = cashgameRepository;
        _playerRepository = playerRepository;
        _userRepository = userRepository;
        _locationRepository = locationRepository;
        _eventRepository = eventRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var cashgame = await _cashgameRepository.Get(request.Id);
        var bunch = await _bunchRepository.Get(cashgame.BunchId);
        var user = await _userRepository.Get(request.UserName);
        var player = await _playerRepository.Get(bunch.Id, user.Id);

        if (!AccessControl.CanSeeCashgame(user, player))
            return Error(new AccessDeniedError());

        var players = await _playerRepository.Get(GetPlayerIds(cashgame));

        var role = user.IsAdmin ? Role.Manager : player.Role;

        var location = await _locationRepository.Get(cashgame.LocationId);
        var @event = cashgame.EventId != 0 ? await _eventRepository.Get(cashgame.EventId) : null;
        var eventName = @event?.Name;
        var eventId = @event?.Id ?? 0;

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
            player.Id,
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
            role,
            cashgame.Status == GameStatus.Running);

        return Success(result);
    }
    
    private DateTime GetStartTime(IList<RunningCashgamePlayerItem> playerItems, DateTime currentUtc)
    {
        if (playerItems.Any())
            return playerItems.Min(o => o.BuyinTime);
        return currentUtc;
    }

    private DateTime GetUpdatedTime(IList<RunningCashgamePlayerItem> playerItems, GameStatus status, DateTime currentUtc)
    {
        if (status == GameStatus.Finished && playerItems.Any())
            return playerItems.Max(o => o.UpdatedTime);
        return currentUtc;
    }

    private static IList<int> GetPlayerIds(Cashgame cashgame)
    {
        return cashgame.Results.Select(o => o.PlayerId).ToList();
    }

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

    public class Request
    {
        public string UserName { get; }
        public int Id { get; }
        public DateTime CurrentUtc { get; }

        public Request(string userName, int id, DateTime currentUtc)
        {
            UserName = userName;
            Id = id;
            CurrentUtc = currentUtc;
        }
    }

    public class Result
    {
        public string Slug { get; }
        public string Timezone { get; }
        public int PlayerId { get; }
        public int CashgameId { get; }
        public DateTime StartTime { get; }
        public DateTime UpdatedTime { get; }
        public string LocationName { get; }
        public int LocationId { get; }
        public string EventName { get; set; }
        public int EventId { get; set; }
        public IList<RunningCashgamePlayerItem> PlayerItems { get; }
        public int DefaultBuyin { get; }
        public string CurrencyFormat { get; }
        public string CurrencySymbol { get; }
        public string CurrencyLayout { get; }
        public string ThousandSeparator { get; }
        public string Culture { get; }
        public Role Role { get; }
        public bool IsRunning { get; }

        public Result(
            string slug,
            string timezone,
            int playerId,
            int cashgameId,
            DateTime startTime,
            DateTime updatedTime,
            string locationName,
            int locationId,
            string eventName,
            int eventId,
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
            Slug = slug;
            Timezone = timezone;
            PlayerId = playerId;
            CashgameId = cashgameId;
            StartTime = startTime;
            UpdatedTime = updatedTime;
            LocationName = locationName;
            LocationId = locationId;
            EventName = eventName;
            EventId = eventId;
            PlayerItems = playerItems;
            DefaultBuyin = defaultBuyin;
            CurrencyFormat = currencyFormat;
            CurrencySymbol = currencySymbol;
            CurrencyLayout = currencyLayout;
            ThousandSeparator = thousandSeparator;
            Culture = culture;
            Role = role;
            IsRunning = isRunning;
        }
    }

    public class RunningCashgameActionItem
    {
        public string Id { get; }
        public CheckpointType Type { get; }
        public DateTime Time { get; }
        public int Stack { get; }
        public int AddedMoney { get; }

        public RunningCashgameActionItem(Checkpoint checkpoint)
        {
            Id = checkpoint.Id.ToString();
            Type = checkpoint.Type;
            Time = checkpoint.Timestamp;
            Stack = checkpoint.Stack;
            AddedMoney = checkpoint.Amount;
        }
    }

    public class RunningCashgamePlayerItem
    {
        public int PlayerId { get; }
        public string Name { get; }
        public string Color { get; }
        public int CashgameId { get; }
        public bool HasCashedOut { get; }
        public int Buyin { get; }
        public int Stack { get; }
        public int Winnings { get; }
        public DateTime BuyinTime { get; }
        public DateTime UpdatedTime { get; }
        public IList<RunningCashgameActionItem> Checkpoints { get; }

        public RunningCashgamePlayerItem(int playerId, string name, string color, int cashgameId, bool hasCashedOut, IList<Checkpoint> checkpoints)
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
            Winnings = Stack - Buyin;
            BuyinTime = checkpoints.First().Timestamp;
            UpdatedTime = lastCheckpoint.Timestamp;
        }
    }
}