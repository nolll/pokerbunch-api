using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class PlayerFacts
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly ICashgameRepository _cashgameRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IUserRepository _userRepository;

        public PlayerFacts(IBunchRepository bunchRepository, ICashgameRepository cashgameRepository, IPlayerRepository playerRepository, IUserRepository userRepository)
        {
            _bunchRepository = bunchRepository;
            _cashgameRepository = cashgameRepository;
            _playerRepository = playerRepository;
            _userRepository = userRepository;
        }

        public Result Execute(Request request)
        {
            var player = _playerRepository.Get(request.PlayerId);
            var user = _userRepository.Get(request.UserName);
            RequireRole.Player(user, player);
            var bunch = _bunchRepository.Get(player.BunchId);
            var cashgames = _cashgameRepository.GetFinished(bunch.Id);

            return new Result(cashgames, player.Id, bunch.Currency);
        }

        public class Request
        {
            public string UserName { get; }
            public int PlayerId { get; }

            public Request(string userName, int playerId)
            {
                UserName = userName;
                PlayerId = playerId;
            }
        }

        public class Result
        {
            public Money Winnings { get; private set; }
            public Money BestResult { get; private set; }
            public Money WorstResult { get; private set; }
            public int GamesPlayed { get; private set; }
            public Time TimePlayed { get; private set; }
            public int BestResultCount { get; private set; }
            public int CurrentStreak { get; private set; }
            public int WinningStreak { get; private set; }
            public int LosingStreak { get; private set; }

            public Result(IEnumerable<Cashgame> cashgames, int playerId, Currency currency)
            {
                var evaluator = new PlayerFactsEvaluator(cashgames, playerId);

                Winnings = new Money(evaluator.Winnings, currency);
                BestResult = new Money(evaluator.BestResult, currency);
                WorstResult = new Money(evaluator.WorstResult, currency);
                GamesPlayed = evaluator.GameCount;
                TimePlayed = Time.FromMinutes(evaluator.MinutesPlayed);
                BestResultCount = evaluator.BestResultCount;
                CurrentStreak = evaluator.CurrentStreak;
                WinningStreak = evaluator.WinningStreak;
                LosingStreak = evaluator.LosingStreak;
            }
        }

        private class PlayerFactsEvaluator
        {
            private readonly IEnumerable<Cashgame> _cashgames;
            private readonly int _playerId;

            public PlayerFactsEvaluator(IEnumerable<Cashgame> cashgames, int playerId)
            {
                _cashgames = FilterCashgames(cashgames, playerId);
                _playerId = playerId;
            }

            public int GameCount
            {
                get { return _cashgames.Count(); }
            }

            public int MinutesPlayed
            {
                get
                {
                    var minutesPlayed = 0;
                    foreach (var cashgame in _cashgames)
                    {
                        var result = cashgame.GetResult(_playerId);
                        if (result != null)
                        {
                            minutesPlayed += result.PlayedTime;
                        }
                    }
                    return minutesPlayed;
                }
            }

            public int Winnings
            {
                get
                {
                    var winnings = 0;
                    foreach (var cashgame in _cashgames)
                    {
                        var result = cashgame.GetResult(_playerId);
                        if (result != null)
                        {
                            winnings += result.Winnings;
                        }
                    }
                    return winnings;
                }
            }

            public int BestResult
            {
                get
                {
                    int? best = null;
                    foreach (var cashgame in _cashgames)
                    {
                        var result = cashgame.GetResult(_playerId);
                        if (!best.HasValue || result != null && result.Winnings > best)
                        {
                            best = result.Winnings;
                        }
                    }
                    return best.HasValue ? best.Value : 0;
                }
            }

            public int WorstResult
            {
                get
                {
                    int? worst = null;
                    foreach (var cashgame in _cashgames)
                    {
                        var result = cashgame.GetResult(_playerId);
                        if (!worst.HasValue || result != null && result.Winnings < worst)
                        {
                            worst = result.Winnings;
                        }
                    }
                    return worst.HasValue ? worst.Value : 0;
                }
            }

            public int BestResultCount
            {
                get
                {
                    var count = 0;
                    foreach (var cashgame in _cashgames)
                    {
                        var result = cashgame.GetBestResult();
                        if (result.PlayerId == _playerId)
                        {
                            count++;
                        }
                    }
                    return count;
                }
            }

            public int CurrentStreak
            {
                get
                {
                    var lastStreak = 0;
                    var currentStreak = 0;
                    var cashgames = _cashgames.Reverse();
                    foreach (var cashgame in cashgames)
                    {
                        var result = cashgame.GetResult(_playerId);
                        if (result.Winnings >= 0)
                        {
                            currentStreak++;
                        }
                        else
                        {
                            currentStreak--;
                        }
                        if (Math.Abs(currentStreak) < Math.Abs(lastStreak))
                        {
                            return lastStreak;
                        }
                        lastStreak = currentStreak;
                    }
                    return lastStreak;
                }
            }

            public int WinningStreak
            {
                get
                {
                    var bestStreak = 0;
                    var currentStreak = 0;
                    foreach (var cashgame in _cashgames)
                    {
                        var result = cashgame.GetResult(_playerId);
                        if (result.Winnings >= 0)
                        {
                            currentStreak++;
                            if (currentStreak > bestStreak)
                            {
                                bestStreak = currentStreak;
                            }
                        }
                        else
                        {
                            currentStreak = 0;
                        }
                    }
                    return bestStreak;
                }
            }

            public int LosingStreak
            {
                get
                {
                    var worstStreak = 0;
                    var currentStreak = 0;
                    foreach (var cashgame in _cashgames)
                    {
                        var result = cashgame.GetResult(_playerId);
                        if (result.Winnings < 0)
                        {
                            currentStreak++;
                            if (currentStreak > worstStreak)
                            {
                                worstStreak = currentStreak;
                            }
                        }
                        else
                        {
                            currentStreak = 0;
                        }
                    }
                    return worstStreak;
                }
            }

            private static IEnumerable<Cashgame> FilterCashgames(IEnumerable<Cashgame> cashgames, int playerId)
            {
                return cashgames.Where(cashgame => cashgame.IsInGame(playerId)).ToList();
            }
        }
    }
}