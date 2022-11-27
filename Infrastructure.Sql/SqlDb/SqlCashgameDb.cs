using System.Data;
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
        SELECT g.cashgame_id, g.bunch_id, g.location_id, ecg.event_id, g.status, g.date
        FROM pb_cashgame g
        LEFT JOIN pb_event_cashgame ecg ON ecg.cashgame_id = g.cashgame_id ";

    private const string SearchSql = @"
        SELECT g.cashgame_id
        FROM pb_cashgame g ";

    private const string SearchByCheckpointSql = @"
        SELECT cp.cashgame_id
        FROM pb_cashgame_checkpoint cp ";
        
    private readonly PostgresDb _db;

    public SqlCashgameDb(PostgresDb db)
    {
        _db = db;
    }

    public async Task<Cashgame> Get(string cashgameId)
    {
        var sql = string.Concat(DataSql, "WHERE g.cashgame_id = @cashgameId ORDER BY g.cashgame_id");
        var parameters = new List<SqlParam>
        {
            new IntParam("@cashgameId", cashgameId)
        };
        var reader = await _db.Query(sql, parameters);
        var rawGame = reader.ReadOne(CreateRawCashgame);
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

    public async Task<IList<string>> FindFinished(string bunchId, int? year = null)
    {
        var sql = string.Concat(SearchSql, "WHERE g.bunch_id = @bunchId AND g.status = @status");
        const int status = (int)GameStatus.Finished;
        var parameters = new List<SqlParam>
        {
            new IntParam("@bunchId", bunchId),
            new IntParam("@status", status)
        };
        if (year.HasValue)
        {
            sql = string.Concat(sql, " AND DATE_PART('year', g.date) = @year");
            parameters.Add(new IntParam("@year", year.Value));
        }
        var reader = await _db.Query(sql, parameters);
        return reader.ReadIntList("cashgame_id").Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByEvent(string eventId)
    {
        var sql = string.Concat(SearchSql, "WHERE g.cashgame_id IN (SELECT ecg.cashgame_id FROM pb_event_cashgame ecg WHERE ecg.event_id = @eventId)");
        var parameters = new List<SqlParam>
        {
            new IntParam("@eventId", eventId)
        };
        var reader = await _db.Query(sql, parameters);
        return reader.ReadIntList("cashgame_id").Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindRunning(string bunchId)
    {
        const int status = (int)GameStatus.Running;
        var sql = string.Concat(SearchSql, "WHERE g.bunch_id = @bunchId AND g.status = @status");
        var parameters = new List<SqlParam>
        {
            new IntParam("@bunchId", bunchId),
            new IntParam("@status", status)
        };
        var reader = await _db.Query(sql, parameters);
        return reader.ReadIntList("cashgame_id").Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByCheckpoint(string checkpointId)
    {
        var sql = string.Concat(SearchByCheckpointSql, "WHERE cp.checkpoint_id = @checkpointId");
        var parameters = new List<SqlParam>
        {
            new IntParam("@checkpointId", checkpointId)
        };
        var reader = await _db.Query(sql, parameters);
        return reader.ReadIntList("cashgame_id").Select(o => o.ToString()).ToList();
    }

    private RawCashgame CreateRawCashgame(IStorageDataReader reader)
    {
        var intEventId = reader.GetIntValue("event_id");


        var id = reader.GetIntValue("cashgame_id").ToString();
        var bunchId = reader.GetIntValue("bunch_id").ToString();
        var locationId = reader.GetIntValue("location_id").ToString();
        var eventId = intEventId == 0 ? null : intEventId.ToString();
        var status = reader.GetIntValue("status");
        var date = TimeZoneInfo.ConvertTimeToUtc(reader.GetDateTimeValue("date"));

        return new RawCashgame(id, bunchId, locationId, eventId, status, date);
    }

    public async Task DeleteGame(string id){
        const string sql = @"
            DELETE FROM pb_cashgame WHERE cashgame_id = @cashgameId";

        var parameters = new List<SqlParam>
        {
            new IntParam("@cashgameId", id)
        };
        await _db.Execute(sql, parameters);
    }
        
    public async Task<string> AddGame(Bunch bunch, Cashgame cashgame)
    {
        var rawCashgame = CreateRawCashgame(cashgame);
        const string sql = @"
            INSERT INTO pb_cashgame (bunch_id, location_id, status, date)
            VALUES (@bunchId, @locationId, @status, @date) RETURNING cashgame_id";

        var timezoneAdjustedDate = TimeZoneInfo.ConvertTime(rawCashgame.Date, bunch.Timezone);
        var parameters = new List<SqlParam>
        {
            new IntParam("@bunchId", bunch.Id),
            new IntParam("@locationId", rawCashgame.LocationId),
            new IntParam("@status", rawCashgame.Status),
            new DateParam("@date", timezoneAdjustedDate.Date)
        };
        return (await _db.Insert(sql, parameters)).ToString();
    }
        
    public async Task UpdateGame(Cashgame cashgame)
    {
        const string sql = @"
            UPDATE pb_cashgame
            SET location_id = @locationId,
                date = @date,
                status = @status
            WHERE cashgame_id = @cashgameId";

        var rawCashgame = CreateRawCashgame(cashgame);
        var parameters = new List<SqlParam>
        {
            new IntParam("@locationId", rawCashgame.LocationId),
            new TimestampParam("@date", rawCashgame.Date),
            new IntParam("@status", rawCashgame.Status),
            new IntParam("@cashgameId", rawCashgame.Id)
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
        await _db.Execute(sql, parameters);
    }
        
    public async Task<IList<string>> FindByPlayerId(string playerId)
    {
        const string sql = @"
            SELECT DISTINCT cashgame_id
            FROM pb_cashgame_checkpoint
            WHERE player_id = @playerId";

        var parameters = new List<SqlParam>
        {
            new IntParam("@playerId", playerId)
        };
        var reader = await _db.Query(sql, parameters);
        return reader.ReadIntList("cashgame_id").Select(o => o.ToString()).ToList();
    }

    private RawCashgame CreateRawCashgame(Cashgame cashgame, GameStatus? status = null)
    {
        var rawStatus = status.HasValue ? (int) status.Value : (int) cashgame.Status;
        var date = cashgame.StartTime ?? DateTime.UtcNow;
            
        return new RawCashgame(cashgame.Id, cashgame.BunchId, cashgame.LocationId, cashgame.EventId, rawStatus, date);
    }

    private static Cashgame CreateCashgame(RawCashgame rawGame, IList<Checkpoint> checkpoints)
    {
        return new Cashgame(rawGame.BunchId, rawGame.LocationId, rawGame.EventId, (GameStatus)rawGame.Status, rawGame.Id, checkpoints);
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
            if (!checkpointMap.TryGetValue(rawGame.Id, out var gameCheckpoints))
            {
                gameCheckpoints = new List<RawCheckpoint>();
            }
            var checkpoints = CreateCheckpoints(gameCheckpoints);
            var cashgame = CreateCashgame(rawGame, checkpoints);
            cashgames.Add(cashgame);
        }
        return cashgames;
    }

    private static IDictionary<string, IList<RawCheckpoint>> GetGameCheckpointMap(IEnumerable<RawCheckpoint> checkpoints)
    {
        var checkpointMap = new Dictionary<string, IList<RawCheckpoint>>();
        foreach (var checkpoint in checkpoints)
        {
            if (!checkpointMap.TryGetValue(checkpoint.CashgameId, out var checkpointList))
            {
                checkpointList = new List<RawCheckpoint>();
                checkpointMap.Add(checkpoint.CashgameId, checkpointList);
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

        var parameters = new List<SqlParam>
        {
            new IntParam("@cashgameId", checkpoint.CashgameId),
            new IntParam("@playerId", checkpoint.PlayerId),
            new IntParam("@type",(int) checkpoint.Type),
            new IntParam("@amount", checkpoint.Amount),
            new IntParam("@stack", checkpoint.Stack),
            new TimestampParam("@timestamp", checkpoint.Timestamp.ToUniversalTime())
        };
        return await _db.Insert(sql, parameters);
    }

    private async Task UpdateCheckpoint(Checkpoint checkpoint)
    {
        const string sql = @"
            UPDATE pb_cashgame_checkpoint
            SET timestamp = @timestamp,
                amount = @amount,
                stack = @stack
            WHERE checkpoint_id = @checkpointId";
        var parameters = new List<SqlParam>
        {
            new TimestampParam("@timestamp", checkpoint.Timestamp),
            new IntParam("@amount", checkpoint.Amount),
            new IntParam("@stack", checkpoint.Stack),
            new IntParam("@checkpointId", checkpoint.Id)
        };
        await _db.Execute(sql, parameters);
    }

    private async Task DeleteCheckpoint(Checkpoint checkpoint)
    {
        const string sql = @"
            DELETE FROM pb_cashgame_checkpoint
            WHERE checkpoint_id = @checkpointId";

        var parameters = new List<SqlParam>
        {
            new IntParam("@checkpointId", checkpoint.Id)
        };
        await _db.Execute(sql, parameters);
    }

    private async Task<IList<RawCheckpoint>> GetCheckpoints(string cashgameId)
    {
        const string sql = @"
            SELECT cp.cashgame_id, cp.checkpoint_id, cp.player_id, cp.type, cp.stack, cp.amount, cp.timestamp
            FROM pb_cashgame_checkpoint cp
            WHERE cp.cashgame_id = @cashgameId
            ORDER BY cp.player_id, cp.timestamp";
        var parameters = new List<SqlParam>
        {
            new IntParam("@cashgameId", cashgameId)
        };
        var reader = await _db.Query(sql, parameters);
        return reader.ReadList(CreateRawCheckpoint);
    }

    private static RawCheckpoint CreateRawCheckpoint(IStorageDataReader reader)
    {
        return new RawCheckpoint(
            reader.GetIntValue("cashgame_id").ToString(),
            reader.GetIntValue("player_id").ToString(),
            reader.GetIntValue("amount"),
            reader.GetIntValue("stack"),
            TimeZoneInfo.ConvertTimeToUtc(reader.GetDateTimeValue("timestamp")),
            reader.GetIntValue("checkpoint_id").ToString(),
            reader.GetIntValue("type"));
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