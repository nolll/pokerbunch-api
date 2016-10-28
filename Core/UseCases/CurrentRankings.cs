using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class CurrentRankings
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly CashgameService _cashgameService;
        private readonly PlayerService _playerService;
        private readonly IUserRepository _userRepository;

        public CurrentRankings(IBunchRepository bunchRepository, CashgameService cashgameService, PlayerService playerService, IUserRepository userRepository)
        {
            _bunchRepository = bunchRepository;
            _cashgameService = cashgameService;
            _playerService = playerService;
            _userRepository = userRepository;
        }

        public Result Execute(Request request)
        {
            var bunch = _bunchRepository.GetBySlug(request.Slug);
            var user = _userRepository.Get(request.UserName);
            var player = _playerService.Get(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var years = _cashgameService.GetYears(bunch.Id);
            var latestYear = years.Count > 0 ? years.OrderBy(o => o).Last() : (int?)null;
            var cashgames = _cashgameService.GetFinished(bunch.Id, latestYear);
            if (!cashgames.Any())
                return new Result(new List<Item>(), 0);

            var players = _playerService.List(bunch.Id).ToList();
            var suite = new CashgameSuite(cashgames, players);
            var lastGame = cashgames.Last();
            var items = CreateItems(bunch, suite, lastGame);
            
            return new Result(items, lastGame.Id);
        }
        
        private IEnumerable<Item> CreateItems(Bunch bunch, CashgameSuite suite, Cashgame lastGame)
        {
            var items = new List<Item>();
            var index = 1;
            foreach (var totalResult in suite.TotalResults)
            {
                var lastGameResult = lastGame.GetResult(totalResult.Player.Id);
                var item = new Item(totalResult, lastGameResult, index++, bunch.Currency);
                items.Add(item);
            }
            return items;
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
            public IList<Item> Items { get; }
            public int LastGameId { get; }
            public bool HasGames => Items.Any();

            public Result(IEnumerable<Item> items, int lastGameId)
            {
                Items = items.ToList();
                LastGameId = lastGameId;
            }
        }

        public class Item
        {
            public int Rank { get; }
            public int PlayerId { get; }
            public string Name { get; }
            public Money TotalWinnings { get; }
            public Money LastGameWinnings { get; }

            public Item(CashgameTotalResult totalResult, CashgameResult lastGameResult, int index, Currency currency)
            {
                Rank = index;
                PlayerId = totalResult.Player.Id;
                Name = totalResult.Player.DisplayName;
                TotalWinnings = new Money(totalResult.Winnings, currency);
                LastGameWinnings = lastGameResult != null ? new Money(lastGameResult.Winnings, currency) : null;
            }
        }
    }
}