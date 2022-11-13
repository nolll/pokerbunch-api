using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Entities;
using Core.Entities.Checkpoints;
using Infrastructure.Sql.Classes;
using Infrastructure.Sql.Interfaces;

namespace Infrastructure.Sql.SqlDb;

public class SqlCashgameDb
{
    private const string DataSql = @"
        SELECT g.game_id, g.bunch_id, g.location_id, ecg.event_id, g.status, g.date
        FROM pb_game g
        LEFT JOIN pb_event_cashgame ecg ON ecg.game_id = g.game_id ";

    private const string SearchSql = @"
        SELECT g.game_id
        FROM pb_game g ";

    private const string SearchByCheckpointSql = @"
        SELECT cp.game_id
        FROM pb_cashgame_checkpoint cp ";
        
    private readonly PostgresStorageProvider _db;

    public SqlCashgameDb(PostgresStorageProvider db)
    {
        _db = db;
    }

    public Cashgame Get(int cashgameId)
    {
        var sql = string.Concat(DataSql, "WHERE g.game_id = @cashgameId ORDER BY g.game_id");
        var parameters = new List<SimpleSqlParameter>
        {
            new("@cashgameId", cashgameId)
        };
        var reader = _db.Query(sql, parameters);
        var rawGame = reader.ReadOne(CreateRawCashgame);
        var rawCheckpoints = GetCheckpoints(cashgameId);
        var checkpoints = CreateCheckpoints(rawCheckpoints);
        return CreateCashgame(rawGame, checkpoints);
    }
        
    public IList<Cashgame> Get(IList<int> ids)
    {
        if(ids.Count == 0)
            return new List<Cashgame>();
        var sql = string.Concat(DataSql, "WHERE g.game_id IN (@idList) ORDER BY g.game_id");
        var parameter = new ListSqlParameter("@idList", ids);
        var reader = _db.Query(sql, parameter);
        var rawCashgames = reader.ReadList(CreateRawCashgame);
        var rawCheckpoints = GetCheckpoints(ids);
        return CreateCashgameList(rawCashgames, rawCheckpoints);
    }

    public IList<int> FindFinished(int bunchId, int? year = null)
    {
        var sql = string.Concat(SearchSql, "WHERE g.bunch_id = @bunchId AND g.status = @status");
        const int status = (int)GameStatus.Finished;
        var parameters = new List<SimpleSqlParameter>
        {
            new("@bunchId", bunchId),
            new("@status", status)
        };
        if (year.HasValue)
        {
            sql = string.Concat(sql, " AND YEAR(g.date) = @year");
            parameters.Add(new SimpleSqlParameter("@year", year.Value));
        }
        var reader = _db.Query(sql, parameters);
        return reader.ReadIntList("game_id");
    }

    public IList<int> FindByEvent(int eventId)
    {
        var sql = string.Concat(SearchSql, "WHERE g.game_id IN (SELECT ecg.game_id FROM pb_event_cashgame ecg WHERE ecg.event_id = @eventId)");
        var parameters = new List<SimpleSqlParameter>
        {
            new("@eventId", eventId),
        };
        var reader = _db.Query(sql, parameters);
        return reader.ReadIntList("game_id");
    }

    public IList<int> FindRunning(int bunchId)
    {
        const int status = (int)GameStatus.Running;
        var sql = string.Concat(SearchSql, "WHERE g.bunch_id = @bunchId AND g.status = @status");
        var parameters = new List<SimpleSqlParameter>
        {
            new("@bunchId", bunchId),
            new("@status", status)
        };
        var reader = _db.Query(sql, parameters);
        return reader.ReadIntList("game_id");
    }

    public IList<int> FindByCheckpoint(int checkpointId)
    {
        var sql = string.Concat(SearchByCheckpointSql, "WHERE cp.checkpoint_id = @checkpointId");
        var parameters = new List<SimpleSqlParameter>
        {
            new("@checkpointId", checkpointId)
        };
        var reader = _db.Query(sql, parameters);
        return reader.ReadIntList("game_id");
    }

    private RawCashgame CreateRawCashgame(IStorageDataReader reader)
    {
        var id = reader.GetIntValue("game_id");
        var bunchId = reader.GetIntValue("bunch_id");
        var locationId = reader.GetIntValue("location_id");
        var eventId = reader.GetIntValue("event_id");
        var status = reader.GetIntValue("status");
        var date = TimeZoneInfo.ConvertTimeToUtc(reader.GetDateTimeValue("date"));

        return new RawCashgame(id, bunchId, locationId, eventId, status, date);
    }

    public IList<int> GetYears(int bunchId)
    {
        const string sql = @"
            SELECT DISTINCT YEAR(g.date) as 'Year'
            FROM pb_game g
            WHERE g.bunch_id = @bunchId
            AND g.status = @status
            ORDER BY 'Year' DESC";
        var parameters = new List<SimpleSqlParameter>
        {
            new("@bunchId", bunchId),
            new("@status", (int) GameStatus.Finished)
        };
        var reader = _db.Query(sql, parameters);
        return reader.ReadIntList("year");
    }

    public void DeleteGame(int id){
        const string sql = @"
            DELETE FROM pb_game WHERE game_id = @cashgameId";

        var parameters = new List<SimpleSqlParameter>
        {
            new("@cashgameId", id)
        };
        _db.Execute(sql, parameters);
    }
        
    public int AddGame(Bunch bunch, Cashgame cashgame)
    {
        var rawCashgame = CreateRawCashgame(cashgame);
        const string sql = @"
            INSERT INTO pb_game (bunch_id, location_id, status, date)
            VALUES (@bunchId, @locationId, @status, @date) RETURNING game_id";

        var timezoneAdjustedDate = TimeZoneInfo.ConvertTime(rawCashgame.Date, bunch.Timezone);
        var parameters = new List<SimpleSqlParameter>
        {
            new("@bunchId", bunch.Id),
            new("@locationId", rawCashgame.LocationId),
            new("@status", rawCashgame.Status),
            new("@date", timezoneAdjustedDate.Date, DbType.Date)
        };
        return _db.ExecuteInsert(sql, parameters);
    }
        
    public void UpdateGame(Cashgame cashgame)
    {
        const string sql = @"
            UPDATE pb_game
            SET location_id = @locationId,
                date = @date,
                status = @status
            WHERE game_id = @cashgameId";

        var rawCashgame = CreateRawCashgame(cashgame);
        var parameters = new List<SimpleSqlParameter>
        {
            new("@locationId", rawCashgame.LocationId),
            new("@date", rawCashgame.Date),
            new("@status", rawCashgame.Status),
            new("@cashgameId", rawCashgame.Id)
        };
        if (cashgame.AddedCheckpoints.Any())
        {
            foreach (var checkpoint in cashgame.AddedCheckpoints)
            {
                AddCheckpoint(checkpoint);
            }
        }
        if (cashgame.UpdatedCheckpoints.Any())
        {
            foreach (var checkpoint in cashgame.UpdatedCheckpoints)
            {
                UpdateCheckpoint(checkpoint);
            }
        }
        if (cashgame.DeletedCheckpoints.Any())
        {
            foreach (var checkpoint in cashgame.DeletedCheckpoints)
            {
                DeleteCheckpoint(checkpoint);
            }
        }
        _db.Execute(sql, parameters);
    }
        
    public IList<int> FindByPlayerId(int playerId)
    {
        const string sql = @"
            SELECT DISTINCT game_id
            FROM pb_cashgame_checkpoint
            WHERE player_id = @playerId";

        var parameters = new List<SimpleSqlParameter>
        {
            new("@playerId", playerId)
        };
        var reader = _db.Query(sql, parameters);
        return reader.ReadIntList("game_id");
    }

    private RawCashgame CreateRawCashgame(Cashgame cashgame, GameStatus? status = null)
    {
        var rawStatus = status.HasValue ? (int) status.Value : (int) cashgame.Status;
        var date = cashgame.StartTime.HasValue ? cashgame.StartTime.Value : DateTime.UtcNow;
            
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
            IList<RawCheckpoint> gameCheckpoints;
            if (!checkpointMap.TryGetValue(rawGame.Id, out gameCheckpoints))
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
            IList<RawCheckpoint> checkpointList;
            if (!checkpointMap.TryGetValue(checkpoint.CashgameId, out checkpointList))
            {
                checkpointList = new List<RawCheckpoint>();
                checkpointMap.Add(checkpoint.CashgameId, checkpointList);
            }
            checkpointList.Add(checkpoint);
        }
        return checkpointMap;
    }

    private int AddCheckpoint(Checkpoint checkpoint)
    {
        const string sql = @"
            INSERT INTO pb_cashgame_checkpoint (game_id, player_id, type, amount, stack, timestamp)
            VALUES (@gameId, @playerId, @type, @amount, @stack, @timestamp) RETURNING checkpoint_id";

        var parameters = new List<SimpleSqlParameter>
        {
            new("@gameId", checkpoint.CashgameId),
            new("@playerId", checkpoint.PlayerId),
            new("@type", (int)checkpoint.Type),
            new("@amount", checkpoint.Amount),
            new("@stack", checkpoint.Stack),
            new("@timestamp", checkpoint.Timestamp.ToUniversalTime())
        };
        return _db.ExecuteInsert(sql, parameters);
    }

    private void UpdateCheckpoint(Checkpoint checkpoint)
    {
        const string sql = @"
            UPDATE pb_cashgame_checkpoint
            SET timestamp = @timestamp,
                amount = @amount,
                stack = @stack
            WHERE checkpoint_id = @checkpointId";
        var parameters = new List<SimpleSqlParameter>
        {
            new("@timestamp", checkpoint.Timestamp),
            new("@amount", checkpoint.Amount),
            new("@stack", checkpoint.Stack),
            new("@checkpointId", checkpoint.Id)
        };
        _db.Execute(sql, parameters);
    }

    private void DeleteCheckpoint(Checkpoint checkpoint)
    {
        const string sql = @"
            DELETE FROM pb_cashgame_checkpoint
            WHERE checkpoint_id = @checkpointId";

        var parameters = new List<SimpleSqlParameter>
        {
            new("@checkpointId", checkpoint.Id)
        };
        _db.Execute(sql, parameters);
    }

    private IList<RawCheckpoint> GetCheckpoints(int cashgameId)
    {
        const string sql = @"
            SELECT cp.game_id, cp.checkpoint_id, cp.player_id, cp.type, cp.stack, cp.amount, cp.timestamp
            FROM pb_cashgame_checkpoint cp
            WHERE cp.game_id = @cashgameId
            ORDER BY cp.player_id, cp.timestamp";
        var parameters = new List<SimpleSqlParameter>
        {
            new("@cashgameId", cashgameId)
        };
        var reader = _db.Query(sql, parameters);
        return reader.ReadList(CreateRawCheckpoint);
    }

    private static RawCheckpoint CreateRawCheckpoint(IStorageDataReader reader)
    {
        return new RawCheckpoint(
            reader.GetIntValue("game_id"),
            reader.GetIntValue("player_id"),
            reader.GetIntValue("amount"),
            reader.GetIntValue("stack"),
            TimeZoneInfo.ConvertTimeToUtc(reader.GetDateTimeValue("timestamp")),
            reader.GetIntValue("checkpoint_id"),
            reader.GetIntValue("type"));
    }

    public IList<RawCheckpoint> GetCheckpoints(IList<int> cashgameIdList)
    {
        const string sql = @"
            SELECT cp.game_id, cp.checkpoint_id, cp.player_id, cp.type, cp.stack, cp.amount, cp.timestamp
            FROM pb_cashgame_checkpoint cp
            WHERE cp.game_id IN (@cashgameIdList)
            ORDER BY cp.player_id, cp.timestamp";

        var parameter = new ListSqlParameter("@cashgameIdList", cashgameIdList);
        var reader = _db.Query(sql, parameter);
        return reader.ReadList(CreateRawCheckpoint);
    }
}