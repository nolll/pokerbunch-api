using System.Linq;
using Core.Entities;
using Core.Entities.Checkpoints;
using Infrastructure.Sql.Classes;
using Infrastructure.Sql.Interfaces;
using Infrastructure.Sql.SqlParameters;

namespace Infrastructure.Sql.SqlDb;

public class SqlCashgameDb
{
    private const string DataSql = @"
        SELECT g.cashgame_id, g.bunch_id, g.location_id, ecg.event_id, g.status
        FROM pb_cashgame g
        LEFT JOIN pb_event_cashgame ecg ON ecg.cashgame_id = g.cashgame_id ";

    private const string SearchSql = @"
        SELECT g.cashgame_id
        FROM pb_cashgame g ";

    private const string SearchByCheckpointSql = @"
        SELECT cp.cashgame_id
        FROM pb_cashgame_checkpoint cp ";
        
    private readonly IDb _db;

    public SqlCashgameDb(IDb db)
    {
        _db = db;
    }

    public async Task<Cashgame> Get(string cashgameId)
    {
        var sql = string.Concat(DataSql, "WHERE g.cashgame_id = @cashgameId ORDER BY g.cashgame_id");
        
        var @params = new
        {
            cashgameId = int.Parse(cashgameId)
        };
        
        var rawGame = await _db.Single<RawCashgame>(sql, @params);
        var rawCheckpoints = await GetCheckpoints(cashgameId);
        var checkpoints = CreateCheckpoints(rawCheckpoints);
        return CreateCashgame(rawGame, checkpoints);
    }
        
    public async Task<IList<Cashgame>> Get(IList<string> ids)
    {
        if(ids.Count == 0)
            return new List<Cashgame>();
        var sql = string.Concat(DataSql, "WHERE g.cashgame_id IN (@idList) ORDER BY g.cashgame_id");
        var parameter = new IntListParam("@idList", ids);
        var reader = await _db.Query(sql, parameter);
        var rawCashgames = reader.ReadList(CreateRawCashgame);
        var rawCheckpoints = await GetCheckpoints(ids);
        return CreateCashgameList(rawCashgames, rawCheckpoints);
    }

    public async Task<IList<string>> FindFinished(string bunchId)
    {
        var sql = string.Concat(SearchSql, "WHERE g.bunch_id = @bunchId AND g.status = @status");

        var @params = new
        {
            bunchId = int.Parse(bunchId),
            status = (int)GameStatus.Finished,
        };

        return (await _db.List<int>(sql, @params)).Select(o => o.ToString()).ToList(); ;
    }

    public async Task<IList<string>> FindFinished(string bunchId, int year)
    {
        var sql = string.Concat(SearchSql, "WHERE g.bunch_id = @bunchId AND g.status = @status AND DATE_PART('year', g.date) = @year");

        var @params = new
        {
            bunchId = int.Parse(bunchId),
            status = (int)GameStatus.Finished,
            year = year
        };
        
        return (await _db.List<int>(sql, @params)).Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByEvent(string eventId)
    {
        var sql = string.Concat(SearchSql, "WHERE g.cashgame_id IN (SELECT ecg.cashgame_id FROM pb_event_cashgame ecg WHERE ecg.event_id = @eventId)");
        
        var @params = new
        {
            eventId = int.Parse(eventId)
        };
        
        return (await _db.List<int>(sql, @params)).Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindRunning(string bunchId)
    {
        var sql = string.Concat(SearchSql, "WHERE g.bunch_id = @bunchId AND g.status = @status");

        var @params = new
        {
            bunchId = int.Parse(bunchId),
            status = (int)GameStatus.Running
        };

        return (await _db.List<int>(sql, @params)).Select(o => o.ToString()).ToList(); ;
    }

    public async Task<IList<string>> FindByCheckpoint(string checkpointId)
    {
        var sql = string.Concat(SearchByCheckpointSql, "WHERE cp.checkpoint_id = @checkpointId");
        
        var @params = new
        {
            checkpointId = int.Parse(checkpointId)
        };
        
        return (await _db.List<int>(sql, @params)).Select(o => o.ToString()).ToList();
    }

    private RawCashgame CreateRawCashgame(IStorageDataReader reader)
    {
        return new RawCashgame
        {
            Cashgame_Id = reader.GetIntValue("cashgame_id"),
            Bunch_Id = reader.GetIntValue("bunch_id"),
            Location_Id = reader.GetIntValue("location_id"),
            Event_Id = reader.GetIntValue("event_id"),
            Status = reader.GetIntValue("status"),
        };
    }

    public async Task DeleteGame(string id){
        const string sql = @"
            DELETE FROM pb_cashgame WHERE cashgame_id = @cashgameId";

        var @params = new
        {
            cashgameId = int.Parse(id)
        };

        await _db.Execute(sql, @params);
    }
        
    public async Task<string> AddGame(Bunch bunch, Cashgame cashgame)
    {
        const string sql = @"
            INSERT INTO pb_cashgame (bunch_id, location_id, status, date)
            VALUES (@bunchId, @locationId, @status, @date) RETURNING cashgame_id";

        var @params = new
        {
            bunchId = int.Parse(bunch.Id),
            locationId = int.Parse(cashgame.LocationId),
            status = (int)cashgame.Status,
            date = DateTime.UtcNow
        };

        return (await _db.Insert(sql, @params)).ToString();
    }
        
    public async Task UpdateGame(Cashgame cashgame)
    {
        const string sql = @"
            UPDATE pb_cashgame
            SET location_id = @locationId, status = @status
            WHERE cashgame_id = @cashgameId";

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
        await _db.Execute(sql, @params);
    }
        
    public async Task<IList<string>> FindByPlayerId(string playerId)
    {
        const string sql = @"
            SELECT DISTINCT cashgame_id
            FROM pb_cashgame_checkpoint
            WHERE player_id = @playerId";

        var @params = new
        {
            playerId = int.Parse(playerId)
        };

        return (await _db.List<int>(sql, @params)).Select(o => o.ToString()).ToList();
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
        const string sql = @"
            INSERT INTO pb_cashgame_checkpoint (cashgame_id, player_id, type, amount, stack, timestamp)
            VALUES (@cashgameId, @playerId, @type, @amount, @stack, @timestamp) RETURNING checkpoint_id";

        var @params = new
        {
            cashgameId = int.Parse(checkpoint.CashgameId),
            playerId = int.Parse(checkpoint.PlayerId),
            type = (int)checkpoint.Type,
            amount = checkpoint.Amount,
            stack = checkpoint.Stack,
            timestamp = checkpoint.Timestamp.ToUniversalTime()
        };

        return await _db.Insert(sql, @params);
    }

    private async Task UpdateCheckpoint(Checkpoint checkpoint)
    {
        const string sql = @"
            UPDATE pb_cashgame_checkpoint
            SET timestamp = @timestamp,
                amount = @amount,
                stack = @stack
            WHERE checkpoint_id = @checkpointId";

        var @params = new
        {
            timestamp = checkpoint.Timestamp,
            amount = checkpoint.Amount,
            stack = checkpoint.Stack,
            checkpointId = int.Parse(checkpoint.Id)
        };

        await _db.Execute(sql, @params);
    }

    private async Task DeleteCheckpoint(Checkpoint checkpoint)
    {
        const string sql = @"
            DELETE FROM pb_cashgame_checkpoint
            WHERE checkpoint_id = @checkpointId";

        var @params = new
        {
            checkpointId = int.Parse(checkpoint.Id)
        };

        await _db.Execute(sql, @params);
    }

    private async Task<IList<RawCheckpoint>> GetCheckpoints(string cashgameId)
    {
        const string sql = @"
            SELECT cp.cashgame_id, cp.checkpoint_id, cp.player_id, cp.type, cp.stack, cp.amount, cp.timestamp
            FROM pb_cashgame_checkpoint cp
            WHERE cp.cashgame_id = @cashgameId
            ORDER BY cp.player_id, cp.timestamp";

        var @params = new
        {
            cashgameId = int.Parse(cashgameId)
        };

        return (await _db.List<RawCheckpoint>(sql, @params)).ToList();
    }

    private static RawCheckpoint CreateRawCheckpoint(IStorageDataReader reader)
    {
        return new RawCheckpoint
        {
            Cashgame_Id = reader.GetIntValue("cashgame_id"),
            Player_Id = reader.GetIntValue("player_id"),
            Amount = reader.GetIntValue("amount"),
            Stack = reader.GetIntValue("stack"),
            Timestamp = reader.GetDateTimeValue("timestamp"),
            Checkpoint_Id = reader.GetIntValue("checkpoint_id"),
            Type = reader.GetIntValue("type")
        };
    }

    private async Task<IList<RawCheckpoint>> GetCheckpoints(IList<string> cashgameIdList)
    {
        const string sql = @"
            SELECT cp.cashgame_id, cp.checkpoint_id, cp.player_id, cp.type, cp.stack, cp.amount, cp.timestamp
            FROM pb_cashgame_checkpoint cp
            WHERE cp.cashgame_id IN (@cashgameIdList)
            ORDER BY cp.player_id, cp.timestamp, cp.checkpoint_id DESC";

        var parameter = new IntListParam("@cashgameIdList", cashgameIdList);
        var reader = await _db.Query(sql, parameter);
        return reader.ReadList(CreateRawCheckpoint);
    }
}