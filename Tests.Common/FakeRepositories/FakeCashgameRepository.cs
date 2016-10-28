using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Entities.Checkpoints;
using Core.Repositories;

namespace Tests.Common.FakeRepositories
{
    public class FakeCashgameRepository : ICashgameRepository
    {
        private IList<Cashgame> _list;
        public Cashgame Added { get; private set; }
        public int Deleted { get; private set; }
        public Cashgame Updated { get; private set; }
        
        public FakeCashgameRepository()
        {
            SetupMultiYear();
}

        public Cashgame Get(int cashgameId)
        {
            return _list.FirstOrDefault(o => o.Id == cashgameId);
        }

        public IList<Cashgame> Get(IList<int> ids)
        {
            return _list.Where(o => ids.Contains(o.Id)).ToList();
        }

        public IList<Cashgame> GetFinished(int bunchId, int? year = null)
        {
            if (year.HasValue)
                return _list.Where(o => o.StartTime.HasValue && o.StartTime.Value.Year == year && o.Status == GameStatus.Finished).ToList();
            return _list.Where(o => o.Status == GameStatus.Finished).ToList();
        }

        public IList<Cashgame> GetByEvent(int eventId)
        {
            throw new NotImplementedException();
        }

        public IList<Cashgame> GetByPlayer(int playerId)
        {
            var ids = new List<Cashgame>();
            foreach (var game in _list)
            {
                if (game.GetResult(playerId) != null)
                {
                    ids.Add(game);
                }
            }
            return ids;
        }

        public Cashgame GetRunning(int bunchId)
        {
            return _list.FirstOrDefault(o => o.Status == GameStatus.Running);
        }

        public Cashgame GetByCheckpoint(int checkpointId)
        {
            return _list.FirstOrDefault(o => o.Checkpoints.Any(p => p.Id == checkpointId));
        }

        public IList<int> GetYears(int bunchId)
        {
            return _list.Where(o => o.StartTime.HasValue && o.Status == GameStatus.Finished).Select(o => o.StartTime.Value.Year).ToList();
        }

        public void DeleteGame(int id)
        {
            Deleted = id;
        }

        public int Add(Bunch bunch, Cashgame cashgame)
        {
            Added = cashgame;
            return 1;
        }

        public void Update(Cashgame cashgame)
        {
            Updated = cashgame;
        }

        public void SetupMultiYear()
        {
            _list = GetGames();
        }

        public void SetupSingleYear()
        {
            _list = GetGames(TestData.StartTimeA.AddDays(7));
        }

        public void SetupGameCount(int gameCount)
        {
            _list = GetGames(gameCount);
        }

        private IList<Cashgame> GetGames(DateTime? secondGameStartTime = null)
        {
            var checkpoints1 = new List<Checkpoint>
            {
                Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdA, TestData.StartTimeA, CheckpointType.Buyin, 200, 200, 1),
                Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdB, TestData.StartTimeA.AddMinutes(1), CheckpointType.Buyin, 200, 200, 2),
                Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdB, TestData.StartTimeA.AddMinutes(30), CheckpointType.Report, 250, 0, 3),
                Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdA, TestData.StartTimeA.AddMinutes(61), CheckpointType.Cashout, 50, 0, 4),
                Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdB, TestData.StartTimeA.AddMinutes(62), CheckpointType.Cashout, 350, 0, 5)
            };

            var startTime2 = secondGameStartTime ?? TestData.StartTimeB;

            var checkpoints2 = new List<Checkpoint>
            {
                Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdA, startTime2, CheckpointType.Buyin, 200, 200, 6),
                Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdB, startTime2.AddMinutes(1), CheckpointType.Buyin, 200, 200, 7),
                Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdB, startTime2.AddMinutes(2), CheckpointType.Buyin, 200, 200, 8),
                Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdA, startTime2.AddMinutes(30), CheckpointType.Report, 450, 45, 9),
                Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdA, startTime2.AddMinutes(91), CheckpointType.Cashout, 550, 0, 10),
                Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdB, startTime2.AddMinutes(92), CheckpointType.Cashout, 50, 0, 11)
            };

            return new List<Cashgame>
            {
                new Cashgame(TestData.BunchA.Id, TestData.LocationIdA, GameStatus.Finished, TestData.CashgameIdA, checkpoints1),
                new Cashgame(TestData.BunchA.Id, TestData.LocationIdB, GameStatus.Finished, TestData.CashgameIdB, checkpoints2)
            };
        }

        private IList<Cashgame> GetGames(int gameCount)
        {
            var games = new List<Cashgame>();
            var startTime = TestData.StartTimeA;
            for (var i = 0; i < gameCount; i++)
            {
                var checkpoints = new List<Checkpoint>
                {
                    Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdA, startTime, CheckpointType.Buyin, 200, 200, 1),
                    Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdA, startTime.AddMinutes(61), CheckpointType.Cashout, 200, 0, 2),
                };
                games.Add(new Cashgame(TestData.BunchA.Id, TestData.LocationIdA, GameStatus.Finished, TestData.CashgameIdA, checkpoints));
                startTime = startTime.AddDays(1);
            }
            return games;
        }

        public void SetupRunningGame()
        {
            _list.Add(new Cashgame(TestData.BunchA.Id, TestData.LocationIdC, GameStatus.Running, TestData.CashgameIdC, TestData.RunningGameCheckpoints));
        }

        public void SetupRunningGameWithCashoutCheckpoint()
        {
            var checkpoints1 = new List<Checkpoint>
            {
                Checkpoint.Create(TestData.CashgameIdC, TestData.PlayerIdA, TestData.StartTimeC, CheckpointType.Buyin, 200, 200, 1),
                Checkpoint.Create(TestData.CashgameIdC, TestData.PlayerIdB, TestData.StartTimeC, CheckpointType.Buyin, 200, 200, 2),
                Checkpoint.Create(TestData.CashgameIdC, TestData.PlayerIdA, TestData.StartTimeC.AddMinutes(1), CheckpointType.Cashout, 200, 0, 3),
            };

            _list.Add(new Cashgame(TestData.BunchA.Id, TestData.LocationIdC, GameStatus.Running, TestData.CashgameIdC, checkpoints1));
        }

        public void SetupEmptyGame()
        {
            var checkpoints1 = new List<Checkpoint>();
            _list.Clear();
            _list.Add(new Cashgame(TestData.BunchA.Id, TestData.LocationIdA, GameStatus.Running, TestData.CashgameIdA, checkpoints1));
        }

        public void ClearList()
        {
            _list.Clear();
        }
    }
}