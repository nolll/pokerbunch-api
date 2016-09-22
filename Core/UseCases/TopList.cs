using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Services;

namespace Core.UseCases
{
    public class TopList
    {
        private readonly BunchService _bunchService;
        private readonly CashgameService _cashgameService;
        private readonly PlayerService _playerService;
        private readonly UserService _userService;

        public TopList(BunchService bunchService, CashgameService cashgameService, PlayerService playerService, UserService userService)
        {
            _bunchService = bunchService;
            _cashgameService = cashgameService;
            _playerService = playerService;
            _userService = userService;
        }

        public Result Execute(Request request)
        {
            var bunch = _bunchService.GetBySlug(request.Slug);
            var user = _userService.GetByNameOrEmail(request.UserName);
            var player = _playerService.GetByUserId(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var cashgames = _cashgameService.GetFinished(bunch.Id, request.Year);
            var players = _playerService.GetList(bunch.Id).ToList();
            var suite = new CashgameSuite(cashgames, players);

            var items = suite.TotalResults.Select((o, index) => new Item(o, index, bunch.Currency));
            items = SortItems(items);

            return new Result(items, bunch.Slug, bunch.Currency.Format, bunch.Currency.ThousandSeparator, request.Year);
        }

        private static IEnumerable<Item> SortItems(IEnumerable<Item> items)
        {
            return items.OrderByDescending(o => o.Winnings);
        }

        public class Request
        {
            public string UserName { get; }
            public string Slug { get; }
            public int? Year { get; }

            public Request(string userName, string slug, int? year)
            {
                UserName = userName;
                Slug = slug;
                Year = year;
            }
        }

        public class Result
        {
            public IList<Item> Items { get; private set; }
            public string Slug { get; private set; }
            public string CurrencyFormat { get; private set; }
            public string ThousandSeparator { get; private set; }
            public int? Year { get; private set; }

            public Result(IEnumerable<Item> items, string slug, string currencyFormat, string thousandSeparator, int? year)
            {
                Items = items.ToList();
                Slug = slug;
                CurrencyFormat = currencyFormat;
                ThousandSeparator = thousandSeparator;
                Year = year;
            }
        }

        public class Item
        {
            public int Rank { get; private set; }
            public int PlayerId { get; private set; }
            public string Name { get; private set; }
            public Money Winnings { get; }
            public Money Buyin { get; }
            public Money Cashout { get; }
            public Time TimePlayed { get; }
            public int GamesPlayed { get; }
            public Money WinRate { get; }

            public Item(CashgameTotalResult totalResult, int index, Currency currency)
            {
                Buyin = new Money(totalResult.Buyin, currency);
                Cashout = new Money(totalResult.Cashout, currency);
                GamesPlayed = totalResult.GameCount;
                TimePlayed = Time.FromMinutes(totalResult.TimePlayed);
                Name = totalResult.Player.DisplayName;
                PlayerId = totalResult.Player.Id;
                Rank = index + 1;
                Winnings = new Money(totalResult.Winnings, currency);
                WinRate = new Money(totalResult.WinRate, currency);
            }
        }
    }
}