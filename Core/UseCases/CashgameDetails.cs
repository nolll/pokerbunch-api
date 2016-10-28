using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class CashgameDetails
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly ICashgameRepository _cashgameRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ILocationRepository _locationRepository;

        public CashgameDetails(IBunchRepository bunchRepository, ICashgameRepository cashgameRepository, IUserRepository userRepository, IPlayerRepository playerRepository, ILocationRepository locationRepository)
        {
            _bunchRepository = bunchRepository;
            _cashgameRepository = cashgameRepository;
            _userRepository = userRepository;
            _playerRepository = playerRepository;
            _locationRepository = locationRepository;
        }

        public Result Execute(Request request)
        {
            var cashgame = _cashgameRepository.Get(request.CashgameId);
            var bunch = _bunchRepository.Get(cashgame.BunchId);
            var user = _userRepository.Get(request.UserName);
            var player = _playerRepository.Get(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var isManager = RoleHandler.IsInRole(user, player, Role.Manager);
            var players = GetPlayers(_playerRepository, cashgame);
            var location = _locationRepository.Get(cashgame.LocationId);

            return new Result(bunch, cashgame, location, players, isManager);
        }

        private static IEnumerable<Player> GetPlayers(IPlayerRepository playerRepository, Cashgame cashgame)
        {
            var playerIds = cashgame.Results.Select(o => o.PlayerId).ToList();
            return playerRepository.Get(playerIds);
        }

        public class Request
        {
            public string UserName { get; }
            public int CashgameId { get; }

            public Request(string userName, int cashgameId)
            {
                UserName = userName;
                CashgameId = cashgameId;
            }
        }

        public class Result
        {
            public Date Date { get; private set; }
            public Time Duration { get; private set; }
            public DateTime? StartTime { get; private set; }
            public DateTime? EndTime { get; private set; }
            public string LocationName { get; private set; }
            public int LocationId { get; private set; }
            public bool CanEdit { get; private set; }
            public string Slug { get; private set; }
            public int CashgameId { get; private set; }
            public IList<PlayerResultItem> PlayerItems { get; private set; }

            public Result(Bunch bunch, Cashgame cashgame, Location location, IEnumerable<Player> players, bool isManager)
            {
                var sortedResults = cashgame.Results.OrderByDescending(o => o.Winnings);

                Date = Date.Parse(cashgame.DateString);
                LocationName = location.Name;
                LocationId = location.Id;
                Duration = Time.FromMinutes(cashgame.Duration);
                StartTime = GetLocalTime(cashgame.StartTime, bunch.Timezone);
                EndTime = GetLocalTime(cashgame.EndTime, bunch.Timezone);
                CanEdit = isManager;
                Slug = bunch.Slug;
                CashgameId = cashgame.Id;
                PlayerItems = sortedResults.Select(o => new PlayerResultItem(bunch, cashgame, GetPlayer(players, o.PlayerId), o)).ToList();
            }

            private static DateTime? GetLocalTime(DateTime? d, TimeZoneInfo timeZone)
            {
                if (!d.HasValue)
                    return null;
                return TimeZoneInfo.ConvertTime(d.Value, timeZone);
            }

            private static Player GetPlayer(IEnumerable<Player> players, int playerId)
            {
                return players.First(o => o.Id == playerId);
            }
        }

        public class PlayerResultItem
        {
            public string Name { get; private set; }
            public string Color { get; private set; }
            public int CashgameId { get; private set; }
            public int PlayerId { get; private set; }
            public Money Buyin { get; private set; }
            public Money Cashout { get; private set; }
            public Money Winnings { get; private set; }
            public Money WinRate { get; private set; }

            public PlayerResultItem(Bunch bunch, Cashgame cashgame, Player player, CashgameResult result)
            {
                Name = player.DisplayName;
                Color = player.Color;
                CashgameId = cashgame.Id;
                PlayerId = player.Id;
                Buyin = new Money(result.Buyin, bunch.Currency);
                Cashout = new Money(result.Stack, bunch.Currency);
                Winnings = new Money(result.Winnings, bunch.Currency);
                WinRate = new Money(result.WinRate, bunch.Currency);
            }
        }
    }
}