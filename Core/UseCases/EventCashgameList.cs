using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class EventCashgameList
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly ICashgameRepository _cashgameRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IEventRepository _eventRepository;

        public EventCashgameList(IBunchRepository bunchRepository, ICashgameRepository cashgameRepository, IUserRepository userRepository, IPlayerRepository playerRepository, ILocationRepository locationRepository, IEventRepository eventRepository)
        {
            _bunchRepository = bunchRepository;
            _cashgameRepository = cashgameRepository;
            _userRepository = userRepository;
            _playerRepository = playerRepository;
            _locationRepository = locationRepository;
            _eventRepository = eventRepository;
        }

        public Result Execute(Request request)
        {
            var @event = _eventRepository.Get(request.EventId);
            var bunch = _bunchRepository.Get(@event.BunchId);
            var user = _userRepository.Get(request.UserName);
            var player = _playerRepository.Get(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var cashgames = _cashgameRepository.GetByEvent(request.EventId);
            cashgames = SortItems(cashgames).ToList();
            var locations = _locationRepository.List(bunch.Id);
            var items = cashgames.Select(o => new Item(bunch, o, GetLocation(o, locations)));
            var role = user.IsAdmin ? Role.Manager : player.Role;

            return new Result(
                bunch.Slug,
                items.ToList(),
                bunch.Currency.Format,
                bunch.Currency.Symbol,
                bunch.Currency.Layout,
                bunch.Currency.ThousandSeparator,
                bunch.Timezone.Id,
                bunch.Currency.Culture.Name,
                role);
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
            public int EventId { get; }

            public Request(string userName, int eventId)
            {
                UserName = userName;
                EventId = eventId;
            }
        }

        public class Result
        {
            public IList<Item> Items { get; }
            public string Slug { get; }
            public string CurrencyFormat { get; }
            public string CurrencySymbol { get; }
            public string CurrencyLayout { get; }
            public string ThousandSeparator { get; }
            public string Timezone { get; }
            public string Culture { get; }
            public Role Role { get; }

            public Result(string slug, IList<Item> items, string currencyFormat, string currencySymbol, string currencyLayout, string thousandSeparator, string timezone, string culture, Role role)
            {
                Slug = slug;
                Items = items;
                CurrencyFormat = currencyFormat;
                CurrencySymbol = currencySymbol;
                CurrencyLayout = currencyLayout;
                ThousandSeparator = thousandSeparator;
                Timezone = timezone;
                Culture = culture;
                Role = role;
            }
        }

        public class Item
        {
            public int LocationId { get; }
            public string LocationName { get; }
            public int CashgameId { get; }
            public Time Duration { get; }
            public Date Date { get; }
            public DateTime StartTime { get; }
            public DateTime EndTime { get; }
            public Money Turnover { get; }
            public Money AverageBuyin { get; }
            public int PlayerCount { get; }
            public IList<ItemPlayer> Players { get; }

            public Item(Bunch bunch, Cashgame cashgame, Location location)
            {
                LocationId = location.Id;
                LocationName = location.Name;
                CashgameId = cashgame.Id;
                Duration = Time.FromMinutes(cashgame.Duration);
                Date = cashgame.StartTime.HasValue ? new Date(cashgame.StartTime.Value) : new Date(DateTime.MinValue);
                StartTime = cashgame.StartTime ?? DateTime.MinValue;
                EndTime = cashgame.EndTime ?? DateTime.MinValue;
                Turnover = new Money(cashgame.Turnover, bunch.Currency);
                AverageBuyin = new Money(cashgame.AverageBuyin, bunch.Currency);
                PlayerCount = cashgame.PlayerCount;
                Players = cashgame.Results.Select(o => new ItemPlayer(o)).ToList();
            }
        }

        public class ItemPlayer
        {
            public int Id { get; }
            public DateTime BuyinTime { get; }
            public DateTime UpdatedTime { get; }
            public int Buyin { get; }
            public int Stack { get; }

            public ItemPlayer(CashgameResult result)
            {
                Id = result.PlayerId;
                BuyinTime = result.BuyinTime ?? DateTime.MinValue;
                UpdatedTime = result.LastReportTime;
                Buyin = result.Buyin;
                Stack = result.Stack;
            }
        }

        public enum SortOrder
        {
            Date,
            PlayerCount,
            Location,
            Duration,
            Turnover,
            AverageBuyin
        }
    }
}