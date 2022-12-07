using System.Linq;
using Core.Entities;
using Core.Entities.Checkpoints;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Sql;

namespace Infrastructure.Sql.SqlDb;

public class CashgameDb
{
    private readonly IDb _db;

    public CashgameDb(IDb db)
    {
        _db = db;
    }

    public async Task<Cashgame> Get(string cashgameId)
    {
        var @params = new
        {
            cashgameId = int.Parse(cashgameId)
        };
        
        var rawGame = await _db.Single<RawCashgame>(CashgameSql.GetByIdQuery, @params);
        var rawCheckpoints = await GetCheckpoints(cashgameId);
        var checkpoints = CreateCheckpoints(rawCheckpoints);
        return CreateCashgame(rawGame, checkpoints);
    }
        
    public async Task<IList<Cashgame>> Get(IList<string> ids)
    {
        if(ids.Count == 0)
            return new List<Cashgame>();
        var param = new ListParam("@ids", ids.Select(int.Parse));
        var rawCashgames = await _db.List<RawCashgame>(CashgameSql.GetByIdsQuery, param);
        var rawCheckpoints = await GetCheckpoints(ids);
        return CreateCashgameList(rawCashgames, rawCheckpoints);
    }

    public async Task<IList<string>> FindFinished(string bunchId)
    {
        return await FindByBunchAndStatus(bunchId, GameStatus.Finished);
    }

    public async Task<IList<string>> FindRunning(string bunchId)
    {
        return await FindByBunchAndStatus(bunchId, GameStatus.Running);
    }

    private async Task<IList<string>> FindByBunchAndStatus(string bunchId, GameStatus status)
    {
        var @params = new
        {
            bunchId = int.Parse(bunchId),
            status = (int)status,
        };

        return (await _db.List<int>(CashgameSql.SearchByBunchAndStatusQuery, @params)).Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindFinished(string bunchId, int year)
    {
        var @params = new
        {
            bunchId = int.Parse(bunchId),
            status = (int)GameStatus.Finished,
            year = year
        };
        
        return (await _db.List<int>(CashgameSql.SearchByBunchAndStatusAndYearQuery(_db.Engine), @params))
            .Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByEvent(string eventId)
    {
        var @params = new
        {
            eventId = int.Parse(eventId)
        };
        
        return (await _db.List<int>(CashgameSql.SearchByEvent, @params)).Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByCheckpoint(string checkpointId)
    {
        var @params = new
        {
            checkpointId = int.Parse(checkpointId)
        };
        
        return (await _db.List<int>(CashgameSql.SearchByCheckpointSql, @params)).Select(o => o.ToString()).ToList();
    }
    
    public async Task DeleteGame(string id)
    {
        var @params = new
        {
            cashgameId = int.Parse(id)
        };

        await _db.Execute(CashgameSql.DeleteQuery, @params);
    }
        
    public async Task<string> AddGame(Bunch bunch, Cashgame cashgame)
    {
        var @params = new
        {
            bunchId = int.Parse(bunch.Id),
            locationId = int.Parse(cashgame.LocationId),
            status = (int)cashgame.Status,
            date = TimeZoneInfo.ConvertTime(DateTime.UtcNow, bunch.Timezone)
        };

        return (await _db.Insert(CashgameSql.AddQuery, @params)).ToString();
    }
        
    public async Task UpdateGame(Cashgame cashgame)
    {
        var @params = new
        {
            locationId = int.Parse(cashgame.LocationId),
            status = (int)cashgame.Status,
            cashgameId = int.Parse(cashgame.Id)
        };

        if (cashgame.AddedCheckpoints.Any())
        {
            foreach (var checkpoint in cashgame.AddedCheckpoints)
            {
                await AddCheckpoint(checkpoint);
            }
        }
        if (cashgame.UpdatedCheckpoints.Any())
        {
            foreach (var checkpoint in cashgame.UpdatedCheckpoints)
            {
                await UpdateCheckpoint(checkpoint);
            }
        }
        if (cashgame.DeletedCheckpoints.Any())
        {
            foreach (var checkpoint in cashgame.DeletedCheckpoints)
            {
                await DeleteCheckpoint(checkpoint);
            }
        }
        await _db.Execute(CashgameSql.UpdateQuery, @params);
    }
        
    public async Task<IList<string>> FindByPlayerId(string playerId)
    {
        var @params = new
        {
            playerId = int.Parse(playerId)
        };

        return (await _db.List<int>(CashgameSql.SearchByPlayerId, @params)).Select(o => o.ToString()).ToList();
    }
    
    private static Cashgame CreateCashgame(RawCashgame rawGame, IList<Checkpoint> checkpoints)
    {
        return new Cashgame(
            rawGame.Bunch_Id.ToString(),
            rawGame.Location_Id.ToString(),
            rawGame.Event_Id != 0 ? rawGame.Event_Id.ToString() : null,
            (GameStatus)rawGame.Status,
            rawGame.Cashgame_Id.ToString(),
            checkpoints);
    }

    private static IList<Checkpoint> CreateCheckpoints(IEnumerable<RawCheckpoint> checkpoints)
    {
        return checkpoints.Select(RawCheckpoint.CreateReal).ToList();
    } 

    private static IList<Cashgame> CreateCashgameList(IEnumerable<RawCashgame> rawGames, IEnumerable<RawCheckpoint> rawCheckpoints)
    {
        var checkpointMap = GetGameCheckpointMap(rawCheckpoints);

        var cashgames = new List<Cashgame>();
        foreach (var rawGame in rawGames)
        {
            if (!checkpointMap.TryGetValue(rawGame.Cashgame_Id, out var gameCheckpoints))
            {
                gameCheckpoints = new List<RawCheckpoint>();
            }
            var checkpoints = CreateCheckpoints(gameCheckpoints);
            var cashgame = CreateCashgame(rawGame, checkpoints);
            cashgames.Add(cashgame);
        }
        return cashgames;
    }

    private static IDictionary<int, IList<RawCheckpoint>> GetGameCheckpointMap(IEnumerable<RawCheckpoint> checkpoints)
    {
        var checkpointMap = new Dictionary<int, IList<RawCheckpoint>>();
        foreach (var checkpoint in checkpoints)
        {
            if (!checkpointMap.TryGetValue(checkpoint.Cashgame_Id, out var checkpointList))
            {
                checkpointList = new List<RawCheckpoint>();
                checkpointMap.Add(checkpoint.Cashgame_Id, checkpointList);
            }
            checkpointList.Add(checkpoint);
        }
        return checkpointMap;
    }

    private async Task<int> AddCheckpoint(Checkpoint checkpoint)
    {
        var @params = new
        {
            cashgameId = int.Parse(checkpoint.CashgameId),
            playerId = int.Parse(checkpoint.PlayerId),
            type = (int)checkpoint.Type,
            amount = checkpoint.Amount,
            stack = checkpoint.Stack,
            timestamp = checkpoint.Timestamp.ToUniversalTime()
        };

        return await _db.Insert(CashgameSql.AddCheckpointQuery, @params);
    }

    private async Task UpdateCheckpoint(Checkpoint checkpoint)
    {
        var @params = new
        {
            timestamp = checkpoint.Timestamp,
            amount = checkpoint.Amount,
            stack = checkpoint.Stack,
            checkpointId = int.Parse(checkpoint.Id)
        };

        await _db.Execute(CashgameSql.UpdateCheckpointQuery, @params);
    }

    private async Task DeleteCheckpoint(Checkpoint checkpoint)
    {
        var @params = new
        {
            checkpointId = int.Parse(checkpoint.Id)
        };

        await _db.Execute(CashgameSql.DeleteCheckpointQuery, @params);
    }

    private async Task<IList<RawCheckpoint>> GetCheckpoints(string cashgameId)
    {
        var @params = new
        {
            cashgameId = int.Parse(cashgameId)
        };

        return (await _db.List<RawCheckpoint>(CashgameSql.GetCheckpointsByCashgameQuery, @params)).ToList();
    }

    private async Task<IList<RawCheckpoint>> GetCheckpoints(IList<string> cashgameIdList)
    {
        var param = new ListParam("@cashgameIds", cashgameIdList.Select(int.Parse));
        return (await _db.List<RawCheckpoint>(CashgameSql.GetCheckpointsByCashgamesQuery, param)).ToList();
    }
}