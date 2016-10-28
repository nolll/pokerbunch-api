using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class CashgameChart
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly CashgameService _cashgameService;
        private readonly IPlayerRepository _playerRepository;
        private readonly IUserRepository _userRepository;

        public CashgameChart(IBunchRepository bunchRepository, CashgameService cashgameService, IPlayerRepository playerRepository, IUserRepository userRepository)
        {
            _bunchRepository = bunchRepository;
            _cashgameService = cashgameService;
            _playerRepository = playerRepository;
            _userRepository = userRepository;
        }

        public Result Execute(Request request)
        {
            var bunch = _bunchRepository.GetBySlug(request.Slug);
            var user = _userRepository.Get(request.UserName);
            var player = _playerRepository.Get(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var players = _playerRepository.List(bunch.Id).OrderBy(o => o.DisplayName).ToList();
            var cashgames = _cashgameService.GetFinished(bunch.Id, request.Year);
            var suite = new CashgameSuite(cashgames, players);

            var playerItems = GetPlayerItems(suite.TotalResults);
            var gameItems = GetGameItems(suite.Cashgames, suite.TotalResults);

            return new Result(gameItems, playerItems);
        }

        private static IList<PlayerItem> GetPlayerItems(IEnumerable<CashgameTotalResult> results)
        {
            return results.Select(result => new PlayerItem(result.Player.Id, result.Player.DisplayName, result.Player.Color)).ToList();
        }

        private static IList<GameItem> GetGameItems(IList<Cashgame> cashgames, IList<CashgameTotalResult> results)
        {
            var playerSum = GetEmptyPlayerSumArray(results);
            var gameItems = new List<GameItem>();
            for (var i = 0; i < cashgames.Count; i++)
            {
                var cashgame = cashgames[cashgames.Count - i - 1];
                var currentSums = new Dictionary<int, int>();
                foreach (var totalResult in results)
                {
                    var singleResult = cashgame.GetResult(totalResult.Player.Id);
                    var playerId = totalResult.Player.Id;
                    if (singleResult != null || i == cashgames.Count - 1)
                    {
                        var res = singleResult != null ? singleResult.Stack - singleResult.Buyin : 0;
                        var sum = playerSum[playerId] + res;

                        playerSum[playerId] = sum;
                        currentSums.Add(playerId, sum.Value);
                    }
                }
                var gameItem = new GameItem(new Date(cashgame.StartTime.Value), currentSums);
                gameItems.Add(gameItem);
            }
            return gameItems;
        }

        private static IDictionary<int, int?> GetEmptyPlayerSumArray(IEnumerable<CashgameTotalResult> results)
        {
            return results.ToDictionary<CashgameTotalResult, int, int?>(result => result.Player.Id, result => 0);
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
            public IList<GameItem> GameItems { get; private set; }
            public IList<PlayerItem> PlayerItems { get; private set; }

            public Result(IList<GameItem> gameItems, IList<PlayerItem> playerItems)
            {
                GameItems = gameItems;
                PlayerItems = playerItems;
            }
        }

        public class GameItem
        {
            public Date Date { get; private set; }
            public IDictionary<int, int> Winnings { get; private set; }

            public GameItem(Date date, IDictionary<int, int> winnings)
            {
                Date = date;
                Winnings = winnings;
            }
        }

        public class PlayerItem
        {
            public int Id { get; private set; }
            public string Name { get; private set; }
            public string Color { get; private set; }

            public PlayerItem(int id, string name, string color)
            {
                Id = id;
                Name = name;
                Color = color;
            }
        }
    }
}
