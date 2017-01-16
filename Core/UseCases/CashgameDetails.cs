﻿using System;
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

            return new Result(
                bunch.Slug,
                player.Id,
                cashgame.Id,
                location.Name,
                location.Id,
                playerItems,
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
            public int Id { get; }

            public Request(string userName, int id)
            {
                UserName = userName;
                Id = id;
            }
        }

        public class Result
        {
            public string Slug { get; private set; }
            public int PlayerId { get; private set; }
            public int CashgameId { get; private set; }
            public string LocationName { get; private set; }
            public int LocationId { get; private set; }
            public IList<RunningCashgamePlayerItem> PlayerItems { get; private set; }
            public int DefaultBuyin { get; private set; }
            public string CurrencyFormat { get; private set; }
            public string ThousandSeparator { get; private set; }
            public bool IsManager { get; private set; }

            public Result(
                string slug,
                int playerId,
                int cashgameId,
                string locationName,
                int locationId,
                IList<RunningCashgamePlayerItem> playerItems,
                int defaultBuyin,
                string currencyFormat,
                string thousandSeparator,
                bool isManager)
            {
                Slug = slug;
                PlayerId = playerId;
                CashgameId = cashgameId;
                LocationName = locationName;
                LocationId = locationId;
                PlayerItems = playerItems;
                DefaultBuyin = defaultBuyin;
                CurrencyFormat = currencyFormat;
                ThousandSeparator = thousandSeparator;
                IsManager = isManager;
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
