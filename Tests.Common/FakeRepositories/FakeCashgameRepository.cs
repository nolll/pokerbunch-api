using System;
using Core.Entities;
using Core.Entities.Checkpoints;
using Core.Repositories;

namespace Tests.Common.FakeRepositories;

public class FakeCashgameRepository : ICashgameRepository
{
    private IList<Cashgame> _list;
    public Cashgame Added { get; private set; }
    public string Deleted { get; private set; }
    public Cashgame Updated { get; private set; }
        
    public FakeCashgameRepository()
    {
        SetupMultiYear();
    }

    public Task<Cashgame> Get(string cashgameId)
    {
        return Task.FromResult(_list.FirstOrDefault(o => o.Id == cashgameId));
    }

    public Task<IList<Cashgame>> Get(IList<string> ids)
    {
        return Task.FromResult<IList<Cashgame>>(_list.Where(o => ids.Contains(o.Id)).ToList());
    }

    public Task<IList<Cashgame>> GetFinished(string bunchId, int? year = null)
    {
        var games = year.HasValue 
            ? _list.Where(o => o.StartTime.HasValue && o.StartTime.Value.Year == year && o.Status == GameStatus.Finished).ToList()
            : _list.Where(o => o.Status == GameStatus.Finished).ToList();

        return Task.FromResult<IList<Cashgame>>(games);
    }

    public Task<IList<Cashgame>> GetByEvent(string eventId)
    {
        throw new NotImplementedException();
    }

    public Task<IList<Cashgame>> GetByPlayer(string playerId)
    {
        var games = _list.Where(game => game.GetResult(playerId) != null).ToList();
        return Task.FromResult<IList<Cashgame>>(games);
    }

    public Task<Cashgame> GetRunning(string bunchId)
    {
        return Task.FromResult(_list.FirstOrDefault(o => o.Status == GameStatus.Running));
    }

    public Task<Cashgame> GetByCheckpoint(string checkpointId)
    {
        return Task.FromResult(_list.FirstOrDefault(o => o.Checkpoints.Any(p => p.Id == checkpointId)));
    }

    public Task DeleteGame(string id)
    {
        Deleted = id;
        return Task.CompletedTask;
    }

    public Task<string> Add(Bunch bunch, Cashgame cashgame)
    {
        Added = cashgame;
        return Task.FromResult("1");
    }

    public Task Update(Cashgame cashgame)
    {
        Updated = cashgame;
        return Task.CompletedTask;
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
            Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdA, TestData.StartTimeA, CheckpointType.Buyin, 200, 200, "1"),
            Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdB, TestData.StartTimeA.AddMinutes(1), CheckpointType.Buyin, 200, 200, "2"),
            Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdB, TestData.StartTimeA.AddMinutes(30), CheckpointType.Report, 250, 0, "3"),
            Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdA, TestData.StartTimeA.AddMinutes(61), CheckpointType.Cashout, 50, 0, "4"),
            Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdB, TestData.StartTimeA.AddMinutes(62), CheckpointType.Cashout, 350, 0, "5")
        };

        var startTime2 = secondGameStartTime ?? TestData.StartTimeB;

        var checkpoints2 = new List<Checkpoint>
        {
            Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdA, startTime2, CheckpointType.Buyin, 200, 200, "6"),
            Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdB, startTime2.AddMinutes(1), CheckpointType.Buyin, 200, 200, "7"),
            Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdB, startTime2.AddMinutes(2), CheckpointType.Buyin, 200, 200, "8"),
            Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdA, startTime2.AddMinutes(30), CheckpointType.Report, 450, 45, "9"),
            Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdA, startTime2.AddMinutes(91), CheckpointType.Cashout, 550, 0, "10"),
            Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdB, startTime2.AddMinutes(92), CheckpointType.Cashout, 50, 0, "11")
        };

        return new List<Cashgame>
        {
            new Cashgame(TestData.BunchA.Id, TestData.LocationIdA, null, GameStatus.Finished, TestData.CashgameIdA, checkpoints1),
            new Cashgame(TestData.BunchA.Id, TestData.LocationIdB, null, GameStatus.Finished, TestData.CashgameIdB, checkpoints2)
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
                Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdA, startTime, CheckpointType.Buyin, 200, 200, "1"),
                Checkpoint.Create(TestData.CashgameIdA, TestData.PlayerIdA, startTime.AddMinutes(61), CheckpointType.Cashout, 200, 0, "2"),
            };
            games.Add(new Cashgame(TestData.BunchA.Id, TestData.LocationIdA, null, GameStatus.Finished, TestData.CashgameIdA, checkpoints));
            startTime = startTime.AddDays(1);
        }
        return games;
    }

    public void SetupRunningGame()
    {
        _list.Add(new Cashgame(TestData.BunchA.Id, TestData.LocationIdC, null, GameStatus.Running, TestData.CashgameIdC, TestData.RunningGameCheckpoints));
    }

    public void SetupRunningGameWithCashoutCheckpoint()
    {
        var checkpoints1 = new List<Checkpoint>
        {
            Checkpoint.Create(TestData.CashgameIdC, TestData.PlayerIdA, TestData.StartTimeC, CheckpointType.Buyin, 200, 200, "1"),
            Checkpoint.Create(TestData.CashgameIdC, TestData.PlayerIdB, TestData.StartTimeC, CheckpointType.Buyin, 200, 200, "2"),
            Checkpoint.Create(TestData.CashgameIdC, TestData.PlayerIdA, TestData.StartTimeC.AddMinutes(1), CheckpointType.Cashout, 200, 0, "3"),
        };

        _list.Add(new Cashgame(TestData.BunchA.Id, TestData.LocationIdC, null, GameStatus.Running, TestData.CashgameIdC, checkpoints1));
    }

    public void SetupEmptyGame()
    {
        var checkpoints1 = new List<Checkpoint>();
        _list.Clear();
        _list.Add(new Cashgame(TestData.BunchA.Id, TestData.LocationIdA, null, GameStatus.Running, TestData.CashgameIdA, checkpoints1));
    }

    public void ClearList()
    {
        _list.Clear();
    }
}