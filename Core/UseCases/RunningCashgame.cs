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
    public class RunningCashgame
    {
        private readonly BunchService _bunchService;
        private readonly CashgameService _cashgameService;
        private readonly PlayerService _playerService;
        private readonly UserService _userService;
        private readonly ILocationRepository _locationRepository;

        public RunningCashgame(BunchService bunchService, CashgameService cashgameService, PlayerService playerService, UserService userService, ILocationRepository locationRepository)
        {
            _bunchService = bunchService;
            _cashgameService = cashgameService;
            _playerService = playerService;
            _userService = userService;
            _locationRepository = locationRepository;
        }

        public Result Execute(Request request)
        {
            var bunch = _bunchService.GetBySlug(request.Slug);
            var cashgame = _cashgameService.GetRunning(bunch.Id);

            if(cashgame == null)
                throw new CashgameNotRunningException();

            var user = _userService.GetByNameOrEmail(request.UserName);
            var player = _playerService.GetByUserId(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var players = _playerService.Get(GetPlayerIds(cashgame));
            var bunchPlayers = _playerService.GetList(bunch.Id);

            var isManager = RoleHandler.IsInRole(user, player, Role.Manager);
            
            var location = _locationRepository.Get(cashgame.LocationId);

            var playerItems = GetPlayerItems(cashgame, players);
            var bunchPlayerItems = bunchPlayers.Select(o => new BunchPlayerItem(o.Id, o.DisplayName, o.Color)).OrderBy(o => o.Name).ToList();
            
            var defaultBuyin = bunch.DefaultBuyin;
            var currencyFormat = bunch.Currency.Format;
            var thousandSeparator = bunch.Currency.ThousandSeparator;

            return new Result(
                bunch.Slug,
                player.Id,
                location.Name,
                location.Id,
                playerItems,
                bunchPlayerItems,
                defaultBuyin,
                currencyFormat,
                thousandSeparator,
                isManager);
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
            public string Slug { get; }

            public Request(string userName, string slug)
            {
                UserName = userName;
                Slug = slug;
            }
        }

        public class Result
        {
            public string Slug { get; private set; }
            public int PlayerId { get; private set; }
            public string LocationName { get; private set; }
            public int LocationId { get; private set; }
            public IList<RunningCashgamePlayerItem> PlayerItems { get; private set; }
            public IList<BunchPlayerItem> BunchPlayerItems { get; private set; }
            public int DefaultBuyin { get; private set; }
            public string CurrencyFormat { get; private set; }
            public string ThousandSeparator { get; private set; }
            public bool IsManager { get; private set; }

            public Result(
                string slug,
                int playerId,
                string locationName,
                int locationId,
                IList<RunningCashgamePlayerItem> playerItems,
                IList<BunchPlayerItem> bunchPlayerItems,
                int defaultBuyin,
                string currencyFormat,
                string thousandSeparator,
                bool isManager)
            {
                Slug = slug;
                PlayerId = playerId;
                LocationName = locationName;
                LocationId = locationId;
                PlayerItems = playerItems;
                BunchPlayerItems = bunchPlayerItems;
                DefaultBuyin = defaultBuyin;
                CurrencyFormat = currencyFormat;
                ThousandSeparator = thousandSeparator;
                IsManager = isManager;
            }
        }

        public class BunchPlayerItem
        {
            public int PlayerId { get; private set; }
            public string Name { get; }
            public string Color { get; private set; }

            public BunchPlayerItem(int playerId, string name, string color)
            {
                PlayerId = playerId;
                Name = name;
                Color = color;
            }
        }

        public class RunningCashgameCheckpointItem
        {
            public DateTime Time { get; private set; }
            public int Stack { get; private set; }
            public int AddedMoney { get; private set; }

            public RunningCashgameCheckpointItem(Checkpoint checkpoint)
            {
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
            public DateTime LastReport { get; set; }
            public IList<RunningCashgameCheckpointItem> Checkpoints { get; private set; }

            public RunningCashgamePlayerItem(int playerId, string name, string color, int cashgameId, bool hasCashedOut, IEnumerable<Checkpoint> checkpoints)
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
                LastReport = lastCheckpoint.Timestamp;
            }
        }
    }
}
