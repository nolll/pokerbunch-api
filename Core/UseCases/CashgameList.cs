using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class CashgameList
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly ICashgameRepository _cashgameRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ILocationRepository _locationRepository;

        public CashgameList(IBunchRepository bunchRepository, ICashgameRepository cashgameRepository, IUserRepository userRepository, IPlayerRepository playerRepository, ILocationRepository locationRepository)
        {
            _bunchRepository = bunchRepository;
            _cashgameRepository = cashgameRepository;
            _userRepository = userRepository;
            _playerRepository = playerRepository;
            _locationRepository = locationRepository;
        }

        public Result Execute(Request request)
        {
            var bunch = _bunchRepository.GetBySlug(request.Slug);
            var user = _userRepository.Get(request.UserName);
            var player = _playerRepository.Get(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var cashgames = _cashgameRepository.GetFinished(bunch.Id, request.Year);
            cashgames = SortItems(cashgames, request.SortOrder).ToList();
            var locations = _locationRepository.List(bunch.Id);
            var items = cashgames.Select(o => new Item(bunch, o, GetLocation(o, locations)));

            return new Result(request.Slug, items.ToList(), request.SortOrder, request.Year, bunch.Currency.Format, bunch.Currency.ThousandSeparator);
        }

        private Location GetLocation(Cashgame cashgame, IEnumerable<Location> locations)
        {
            return locations.First(o => o.Id == cashgame.LocationId);
        }

        private static IEnumerable<Cashgame> SortItems(IEnumerable<Cashgame> items, SortOrder orderBy)
        {
            switch (orderBy)
            {
                case SortOrder.PlayerCount:
                    return items.OrderByDescending(o => o.PlayerCount);
                case SortOrder.Duration:
                    return items.OrderByDescending(o => o.Duration);
                case SortOrder.Turnover:
                    return items.OrderByDescending(o => o.Turnover);
                case SortOrder.AverageBuyin:
                    return items.OrderByDescending(o => o.AverageBuyin);
                default:
                    return items.OrderByDescending(o => o.StartTime);
            }
        }

        public class Request
        {
            public string UserName { get; }
            public string Slug { get; }
            public SortOrder SortOrder { get; }
            public int? Year { get; }

            public Request(string userName, string slug, SortOrder sortOrder, int? year)
            {
                UserName = userName;
                Slug = slug;
                SortOrder = sortOrder;
                Year = year;
            }
        }

        public class Result
        {
            public IList<Item> Items { get; private set; }
            public SortOrder SortOrder { get; private set; }
            public string Slug { get; private set; }
            public int? Year { get; private set; }
            public bool ShowYear { get; private set; }
            public string CurrencyFormat { get; private set; }
            public string ThousandSeparator { get; private set; }

            public Result(string slug, IList<Item> items, SortOrder sortOrder, int? year, string currencyFormat, string thousandSeparator)
            {
                Slug = slug;
                Items = items;
                SortOrder = sortOrder;
                Year = year;
                ShowYear = year.HasValue;
                CurrencyFormat = currencyFormat;
                ThousandSeparator = thousandSeparator;
            }
        }

        public class Item
        {
            public string Location { get; private set; }
            public int CashgameId { get; private set; }
            public Time Duration { get; private set; }
            public Date Date { get; private set; }
            public DateTime StartTime { get; }
            public DateTime EndTime { get; }
            public Money Turnover { get; private set; }
            public Money AverageBuyin { get; private set; }
            public int PlayerCount { get; private set; }
            public IList<ItemPlayer> Players { get; }

        public Item(Bunch bunch, Cashgame cashgame, Location location)
            {
                Location = location.Name;
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
            public DateTime LastActionTime { get; }
            public int Buyin { get; }
            public int Cashout { get; }

            public ItemPlayer(CashgameResult result)
            {
                Id = result.PlayerId;
                BuyinTime = result.BuyinTime ?? DateTime.MinValue;
                LastActionTime = result.LastReportTime;
                Buyin = result.Buyin;
                Cashout = result.Stack;
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