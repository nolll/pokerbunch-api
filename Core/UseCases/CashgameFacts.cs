using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class CashgameFacts
    {
        private readonly BunchService _bunchService;
        private readonly CashgameService _cashgameService;
        private readonly PlayerService _playerService;
        private readonly IUserRepository _userRepository;

        public CashgameFacts(BunchService bunchService, CashgameService cashgameService, PlayerService playerService, IUserRepository userRepository)
        {
            _bunchService = bunchService;
            _cashgameService = cashgameService;
            _playerService = playerService;
            _userRepository = userRepository;
        }

        public Result Execute(Request request)
        {
            var bunch = _bunchService.GetBySlug(request.Slug);
            var user = _userRepository.Get(request.UserName);
            var player = _playerService.GetByUserId(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var players = _playerService.GetList(bunch.Id).OrderBy(o => o.DisplayName).ToList();
            var cashgames = _cashgameService.GetFinished(bunch.Id, request.Year);
            var factBuilder = new FactBuilder(cashgames, players);

            return GetFactsResult(_playerService, bunch, factBuilder);
        }

        private static Result GetFactsResult(PlayerService playerService, Bunch bunch, FactBuilder factBuilder)
        {
            var gameCount = factBuilder.GameCount;
            var timePlayed = Time.FromMinutes(factBuilder.TotalGameTime);
            var turnover = new Money(factBuilder.TotalTurnover, bunch.Currency);
            var bestResult = GetBestResult(playerService, factBuilder, bunch.Currency);
            var worstResult = GetWorstResult(playerService, factBuilder, bunch.Currency);
            var bestTotalResult = GetBestTotalResult(factBuilder, bunch.Currency);
            var worstTotalResult = GetWorstTotalResult(factBuilder, bunch.Currency);
            var mostTimeResult = GetMostTimeResult(factBuilder);
            var biggestTotalBuyin = GetBiggestTotalBuyin(factBuilder, bunch.Currency);
            var biggestTotalCashout = GetBiggestTotalCashout(factBuilder, bunch.Currency);

            return new Result(gameCount, timePlayed, turnover, bestResult, worstResult, bestTotalResult, worstTotalResult, mostTimeResult, biggestTotalBuyin, biggestTotalCashout);
        }

        private static MoneyFact GetBestResult(PlayerService playerService, FactBuilder facts, Currency currency)
        {
            var playerName = GetPlayerName(playerService, facts.BestResult.PlayerId);
            var amount = new Money(facts.BestResult.Winnings, currency);
            return new MoneyFact(playerName, amount);
        }

        private static MoneyFact GetWorstResult(PlayerService playerService, FactBuilder facts, Currency currency)
        {
            var playerName = GetPlayerName(playerService, facts.WorstResult.PlayerId);
            var amount = new Money(facts.WorstResult.Winnings, currency);
            return new MoneyFact(playerName, amount);
        }

        private static MoneyFact GetBestTotalResult(FactBuilder facts, Currency currency)
        {
            var amount = new Money(facts.BestTotalResult.Winnings, currency);
            return new MoneyFact(facts.BestTotalResult.Player.DisplayName, amount);
        }

        private static MoneyFact GetWorstTotalResult(FactBuilder facts, Currency currency)
        {
            var amount = new Money(facts.WorstTotalResult.Winnings, currency);
            return new MoneyFact(facts.WorstTotalResult.Player.DisplayName, amount);
        }

        private static DurationFact GetMostTimeResult(FactBuilder facts)
        {
            var timePlayed = Time.FromMinutes(facts.MostTimeResult.TimePlayed);
            return new DurationFact(facts.MostTimeResult.Player.DisplayName, timePlayed);
        }

        private static MoneyFact GetBiggestTotalBuyin(FactBuilder facts, Currency currency)
        {
            var amount = new Money(facts.BiggestBuyinTotalResult.Buyin, currency);
            return new MoneyFact(facts.BiggestBuyinTotalResult.Player.DisplayName, amount);
        }

        private static MoneyFact GetBiggestTotalCashout(FactBuilder facts, Currency currency)
        {
            var amount = new Money(facts.BiggestCashoutTotalResult.Cashout, currency);
            return new MoneyFact(facts.BiggestCashoutTotalResult.Player.DisplayName, amount);
        }

        private static string GetPlayerName(PlayerService playerService, int playerId)
        {
            var player = playerService.Get(playerId);
            return player == null ? string.Empty : player.DisplayName;
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
            public int GameCount { get; private set; }
            public Time TotalTimePlayed { get; private set; }
            public Money Turnover { get; private set; }
            public MoneyFact BestResult { get; private set; }
            public MoneyFact WorstResult { get; private set; }
            public MoneyFact BestTotalResult { get; private set; }
            public MoneyFact WorstTotalResult { get; private set; }
            public DurationFact MostTimePlayed { get; private set; }
            public MoneyFact BiggestBuyin { get; private set; }
            public MoneyFact BiggestCashout { get; private set; }

            public Result(int gameCount, Time totalTimePlayed, Money turnover, MoneyFact bestResult, MoneyFact worstResult, MoneyFact bestTotalResult, MoneyFact worstTotalResult, DurationFact mostTimePlayed, MoneyFact biggestBuyin, MoneyFact biggestCashout)
            {
                GameCount = gameCount;
                TotalTimePlayed = totalTimePlayed;
                Turnover = turnover;
                BestResult = bestResult;
                WorstResult = worstResult;
                BestTotalResult = bestTotalResult;
                WorstTotalResult = worstTotalResult;
                MostTimePlayed = mostTimePlayed;
                BiggestBuyin = biggestBuyin;
                BiggestCashout = biggestCashout;
            }
        }

        public class DurationFact : PlayerFact
        {
            public Time Time { get; private set; }

            public DurationFact(string playerName, Time time)
                : base(playerName)
            {
                Time = time;
            }
        }

        public class MoneyFact : PlayerFact
        {
            public Money Amount { get; private set; }

            public MoneyFact(string playerName, Money amount)
                : base(playerName)
            {
                Amount = amount;
            }
        }

        public class PlayerFact
        {
            public string PlayerName { get; private set; }

            protected PlayerFact(string playerName)
            {
                PlayerName = playerName;
            }
        }

        private class FactBuilder
        {
            public int GameCount { get; }
            public CashgameResult BestResult { get; }
            public CashgameResult WorstResult { get; }
            public CashgameTotalResult BestTotalResult { get; }
            public CashgameTotalResult WorstTotalResult { get; }
            public CashgameTotalResult MostTimeResult { get; }
            public CashgameTotalResult BiggestBuyinTotalResult { get; }
            public CashgameTotalResult BiggestCashoutTotalResult { get; }
            public int TotalGameTime { get; }
            public int TotalTurnover { get; }

            public FactBuilder(IList<Cashgame> cashgames, IEnumerable<Player> players)
            {
                var gameData = GetGameData(cashgames);
                var totalResults = GetTotalResults(players, cashgames);

                GameCount = gameData.GameCount;
                BestResult = gameData.BestResult;
                WorstResult = gameData.WorstResult;
                TotalGameTime = gameData.TotalGameTime;
                TotalTurnover = gameData.TotalTurnover;
                MostTimeResult = GetMostTimeResult(totalResults);
                BiggestBuyinTotalResult = GetBiggestTotalBuyinResult(totalResults);
                BiggestCashoutTotalResult = GetBiggestTotalCashoutResult(totalResults);
                BestTotalResult = totalResults.FirstOrDefault();
                WorstTotalResult = totalResults.LastOrDefault();
            }

            private GameData GetGameData(IList<Cashgame> cashgames)
            {
                CashgameResult bestResult = null;
                CashgameResult worstResult = null;
                var totalGameTime = 0;
                var totalTurnover = 0;

                foreach (var cashgame in cashgames)
                {
                    var results = cashgame.Results;
                    foreach (var result in results)
                    {
                        if (bestResult == null || result.Winnings > bestResult.Winnings)
                        {
                            bestResult = result;
                        }
                        if (worstResult == null || result.Winnings < worstResult.Winnings)
                        {
                            worstResult = result;
                        }
                    }
                    totalGameTime += cashgame.Duration;
                    totalTurnover += cashgame.Turnover;
                }

                return new GameData(cashgames.Count(), bestResult, worstResult, totalGameTime, totalTurnover);
            }

            private class GameData
            {
                public int GameCount { get; }
                public CashgameResult BestResult { get; }
                public CashgameResult WorstResult { get; }
                public int TotalGameTime { get; }
                public int TotalTurnover { get; }

                public GameData(int gameCount, CashgameResult bestResult, CashgameResult worstResult, int totalGameTime, int totalTurnover)
                {
                    GameCount = gameCount;
                    BestResult = bestResult;
                    WorstResult = worstResult;
                    TotalGameTime = totalGameTime;
                    TotalTurnover = totalTurnover;
                }
            }

            private IList<CashgameTotalResult> GetTotalResults(IEnumerable<Player> players,
                IEnumerable<Cashgame> cashgames)
            {
                var list = players.Select(player => new CashgameTotalResult(player, cashgames)).ToList();
                return list.Where(o => o.GameCount > 0).OrderByDescending(o => o.Winnings).ToList();
            }

            private CashgameTotalResult GetMostTimeResult(IEnumerable<CashgameTotalResult> results)
            {
                CashgameTotalResult mostTimeResult = null;
                foreach (var result in results)
                {
                    if (mostTimeResult == null || result.TimePlayed > mostTimeResult.TimePlayed)
                    {
                        mostTimeResult = result;
                    }
                }
                return mostTimeResult;
            }

            private CashgameTotalResult GetBiggestTotalBuyinResult(IEnumerable<CashgameTotalResult> results)
            {
                CashgameTotalResult biggestTotalBuyinResult = null;
                foreach (var result in results)
                {
                    if (biggestTotalBuyinResult == null || result.Buyin > biggestTotalBuyinResult.Buyin)
                    {
                        biggestTotalBuyinResult = result;
                    }
                }
                return biggestTotalBuyinResult;
            }

            private CashgameTotalResult GetBiggestTotalCashoutResult(IEnumerable<CashgameTotalResult> results)
            {
                CashgameTotalResult biggestTotalCashoutResult = null;
                foreach (var result in results)
                {
                    if (biggestTotalCashoutResult == null || result.Cashout > biggestTotalCashoutResult.Cashout)
                    {
                        biggestTotalCashoutResult = result;
                    }
                }
                return biggestTotalCashoutResult;
            }
        }
    }
}