using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Entities.Checkpoints;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class CashgameDetails
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly ICashgameRepository _cashgameRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILocationRepository _locationRepository;

        public CashgameDetails(IBunchRepository bunchRepository, ICashgameRepository cashgameRepository, IPlayerRepository playerRepository, IUserRepository userRepository, ILocationRepository locationRepository)
        {
            _bunchRepository = bunchRepository;
            _cashgameRepository = cashgameRepository;
            _playerRepository = playerRepository;
            _userRepository = userRepository;
            _locationRepository = locationRepository;
        }

        public Result Execute(Request request)
        {
            var cashgame = _cashgameRepository.Get(request.Id);
            var bunch = _bunchRepository.Get(cashgame.BunchId);

            if (cashgame == null)
                throw new CashgameNotRunningException();

            var user = _userRepository.Get(request.UserName);
            var player = _playerRepository.Get(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var players = _playerRepository.Get(GetPlayerIds(cashgame));

            var isManager = RoleHandler.IsInRole(user, player, Role.Manager);
            
            var location = _locationRepository.Get(cashgame.LocationId);

            var playerItems = GetPlayerItems(cashgame, players);
            
            var defaultBuyin = bunch.DefaultBuyin;
            var currencyFormat = bunch.Currency.Format;
            var thousandSeparator = bunch.Currency.ThousandSeparator;

            var startTime = GetStartTime(playerItems, request.CurrentUtc);
            var endTime = GetEndTime(playerItems, cashgame.Status);

            return new Result(
                bunch.Slug,
                bunch.Timezone.Id,
                player.Id,
                cashgame.Id,
                startTime,
                endTime,
                location.Name,
                location.Id,
                playerItems,
                defaultBuyin,
                currencyFormat,
                thousandSeparator,
                isManager,
                cashgame.Status == GameStatus.Running);
        }

        private DateTime GetStartTime(IList<RunningCashgamePlayerItem> playerItems, DateTime currentUtc)
        {
            if (playerItems.Any())
                return playerItems.Min(o => o.BuyinTime);
            return currentUtc;
        }

        private DateTime? GetEndTime(IList<RunningCashgamePlayerItem> playerItems, GameStatus status)
        {
            if (status == GameStatus.Finished && playerItems.Any())
                return playerItems.Max(o => o.LastActionTime);
            return null;
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
            public DateTime? EndTime { get; }
            public string LocationName { get; }
            public int LocationId { get; }
            public IList<RunningCashgamePlayerItem> PlayerItems { get; }
            public int DefaultBuyin { get; }
            public string CurrencyFormat { get; }
            public string ThousandSeparator { get; }
            public bool IsManager { get; }
            public bool IsRunning { get; }

            public Result(
                string slug,
                string timezone,
                int playerId,
                int cashgameId,
                DateTime startTime,
                DateTime? endTime,
                string locationName,
                int locationId,
                IList<RunningCashgamePlayerItem> playerItems,
                int defaultBuyin,
                string currencyFormat,
                string thousandSeparator,
                bool isManager,
                bool isRunning)
            {
                Slug = slug;
                Timezone = timezone;
                PlayerId = playerId;
                CashgameId = cashgameId;
                StartTime = startTime;
                EndTime = endTime;
                LocationName = locationName;
                LocationId = locationId;
                PlayerItems = playerItems;
                DefaultBuyin = defaultBuyin;
                CurrencyFormat = currencyFormat;
                ThousandSeparator = thousandSeparator;
                IsManager = isManager;
                IsRunning = isRunning;
            }
        }

        public class RunningCashgameCheckpointItem
        {
            public CheckpointType Type { get; private set; }
            public DateTime Time { get; private set; }
            public int Stack { get; private set; }
            public int AddedMoney { get; private set; }

            public RunningCashgameCheckpointItem(Checkpoint checkpoint)
            {
                Type = checkpoint.Type;
                Time = checkpoint.Timestamp;
                Stack = checkpoint.Stack;
                AddedMoney = checkpoint.Amount;
            }
        }

        public class RunningCashgamePlayerItem
        {
            public int PlayerId { get; private set; }
            public string Name { get; private set; }
            public string Color { get; private set; }
            public int CashgameId { get; private set; }
            public bool HasCashedOut { get; private set; }
            public int Buyin { get; }
            public int Stack { get; }
            public int Winnings { get; private set; }
            public DateTime BuyinTime { get; set; }
            public DateTime LastActionTime { get; set; }
            public IList<RunningCashgameCheckpointItem> Checkpoints { get; private set; }

            public RunningCashgamePlayerItem(int playerId, string name, string color, int cashgameId, bool hasCashedOut, IList<Checkpoint> checkpoints)
            {
                PlayerId = playerId;
                Name = name;
                Color = color;
                CashgameId = cashgameId;
                HasCashedOut = hasCashedOut;
                var list = checkpoints.ToList();
                var lastCheckpoint = list.Last();
                Checkpoints = list.Select(o => new RunningCashgameCheckpointItem(o)).ToList();
                Buyin = list.Sum(o => o.Amount);
                Stack = lastCheckpoint.Stack;
                Winnings = Stack - Buyin;
                BuyinTime = checkpoints.First().Timestamp;
                LastActionTime = lastCheckpoint.Timestamp;
            }
        }
    }
}
