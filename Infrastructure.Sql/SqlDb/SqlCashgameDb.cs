using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Entities.Checkpoints;
using Infrastructure.Sql.Classes;
using Infrastructure.Sql.Interfaces;

namespace Infrastructure.Sql.SqlDb
{
    public class SqlCashgameDb
    {
        private const string DataSql = "SELECT g.GameID, g.HomegameID, g.LocationId, g.Status, g.Date FROM game g ";
        private const string SearchSql = "SELECT g.GameID FROM game g ";
        private const string SearchByCheckpointSql = "SELECT cp.GameID FROM CashgameCheckpoint cp ";
        
        private readonly SqlServerStorageProvider _db;

	    public SqlCashgameDb(SqlServerStorageProvider db)
	    {
	        _db = db;
	    }

        public Cashgame Get(int cashgameId)
        {
            var sql = string.Concat(DataSql, "WHERE g.GameID = @cashgameId ORDER BY g.GameId");
            var parameters = new List<SimpleSqlParameter>
            {
                new SimpleSqlParameter("@cashgameId", cashgameId)
            };
            var reader = _db.Query(sql, parameters);
            var rawGame = reader.ReadOne(CreateRawCashgame);
            var rawCheckpoints = GetCheckpoints(cashgameId);
            var checkpoints = CreateCheckpoints(rawCheckpoints);
            var cashgame = CreateCashgame(rawGame);
            cashgame.AddCheckpoints(checkpoints);
            return cashgame;
        }
        
        public IList<Cashgame> Get(IList<int> ids)
	    {
            if(ids.Count == 0)
                return new List<Cashgame>();
            var sql = string.Concat(DataSql, "WHERE g.GameID IN (@idList) ORDER BY g.GameID");
            var parameter = new ListSqlParameter("@idList", ids);
            var reader = _db.Query(sql, parameter);
            var rawCashgames = reader.ReadList(CreateRawCashgame);
            var rawCheckpoints = GetCheckpoints(ids);
            return CreateCashgameList(rawCashgames, rawCheckpoints);
	    }

	    public IList<int> FindFinished(int bunchId, int? year = null)
	    {
            var sql = string.Concat(SearchSql, "WHERE g.HomegameID = @homegameId AND g.Status = @status");
            const int status = (int)GameStatus.Finished;
            var parameters = new List<SimpleSqlParameter>
            {
                new SimpleSqlParameter("@homegameId", bunchId),
                new SimpleSqlParameter("@status", status)
            };
            if (year.HasValue)
            {
                sql = string.Concat(sql, " AND YEAR(g.Date) = @year");
                parameters.Add(new SimpleSqlParameter("@year", year.Value));
            }
            var reader = _db.Query(sql, parameters);
            return reader.ReadIntList("GameID");
	    }

	    public IList<int> FindByEvent(int eventId)
	    {
            var sql = string.Concat(SearchSql, "WHERE g.GameID IN (SELECT ecg.GameID FROM eventcashgame ecg WHERE ecg.EventId = @eventId)");
	        var parameters = new List<SimpleSqlParameter>
	        {
	            new SimpleSqlParameter("@eventId", eventId),
	        };
            var reader = _db.Query(sql, parameters);
            return reader.ReadIntList("GameID");
	    }

	    public IList<int> FindRunning(int bunchId)
	    {
            const int status = (int)GameStatus.Running;
            var sql = string.Concat(SearchSql, "WHERE g.HomegameID = @homegameId AND g.Status = @status");
	        var parameters = new List<SimpleSqlParameter>
	        {
	            new SimpleSqlParameter("@homegameId", bunchId),
	            new SimpleSqlParameter("@status", status)
	        };
            var reader = _db.Query(sql, parameters);
            return reader.ReadIntList("GameID");
	    }

	    public IList<int> FindByCheckpoint(int checkpointId)
	    {
            var sql = string.Concat(SearchByCheckpointSql, "WHERE cp.CheckpointID = @checkpointId");
	        var parameters = new List<SimpleSqlParameter>
	        {
	            new SimpleSqlParameter("@checkpointId", checkpointId)
	        };
            var reader = _db.Query(sql, parameters);
            return reader.ReadIntList("GameID");
	    }

	    private RawCashgame CreateRawCashgame(IStorageDataReader reader)
        {
            var id = reader.GetIntValue("GameID");
            var bunchId = reader.GetIntValue("HomegameID");
            var locationId = reader.GetIntValue("LocationId");
            var status = reader.GetIntValue("Status");
            var date = TimeZoneInfo.ConvertTimeToUtc(reader.GetDateTimeValue("Date"));

            return new RawCashgame(id, bunchId, locationId, status, date);
        }

        public IList<int> GetYears(int bunchId)
        {
            const string sql = "SELECT DISTINCT YEAR(g.[Date]) as 'Year' FROM Game g WHERE g.HomegameID = @homegameId AND g.Status = @status ORDER BY 'Year' DESC";
            var parameters = new List<SimpleSqlParameter>
            {
                new SimpleSqlParameter("@homegameId", bunchId),
                new SimpleSqlParameter("@status", (int) GameStatus.Finished)
            };
            var reader = _db.Query(sql, parameters);
            return reader.ReadIntList("Year");
        }

		public void DeleteGame(int id){
            const string sql = "DELETE FROM game WHERE GameID = @cashgameId";
		    var parameters = new List<SimpleSqlParameter>
		    {
		        new SimpleSqlParameter("@cashgameId", id)
		    };
            _db.Execute(sql, parameters);
		}
        
		public int AddGame(Bunch bunch, Cashgame cashgame)
		{
		    var rawCashgame = CreateRawCashgame(cashgame);
            const string sql = "INSERT INTO game (HomegameID, LocationId, Status, Date) VALUES (@homegameId, @locationId, @status, @date) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
            var timezoneAdjustedDate = TimeZoneInfo.ConvertTime(rawCashgame.Date, bunch.Timezone);
		    var parameters = new List<SimpleSqlParameter>
		    {
		        new SimpleSqlParameter("@homegameId", bunch.Id),
		        new SimpleSqlParameter("@locationId", rawCashgame.LocationId),
		        new SimpleSqlParameter("@status", rawCashgame.Status),
		        new SimpleSqlParameter("@date", timezoneAdjustedDate)
		    };
            return _db.ExecuteInsert(sql, parameters);
		}
        
		public void UpdateGame(Cashgame cashgame)
        {
            const string sql = "UPDATE game SET LocationId = @locationId, Date = @date, Status = @status WHERE GameID = @cashgameId";
            var rawCashgame = CreateRawCashgame(cashgame);
		    var parameters = new List<SimpleSqlParameter>
		    {
		        new SimpleSqlParameter("@locationId", rawCashgame.LocationId),
		        new SimpleSqlParameter("@date", rawCashgame.Date),
		        new SimpleSqlParameter("@status", rawCashgame.Status),
		        new SimpleSqlParameter("@cashgameId", rawCashgame.Id)
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
            const string sql = "SELECT DISTINCT GameID FROM cashgamecheckpoint WHERE PlayerId = @playerId";
            var parameters = new List<SimpleSqlParameter>
            {
                new SimpleSqlParameter("@playerId", playerId)
            };
            var reader = _db.Query(sql, parameters);
            return reader.ReadIntList("GameID");
        }

        private RawCashgame CreateRawCashgame(Cashgame cashgame, GameStatus? status = null)
	    {
	        var rawStatus = status.HasValue ? (int) status.Value : (int) cashgame.Status;
	        var date = cashgame.StartTime.HasValue ? cashgame.StartTime.Value : DateTime.UtcNow;
            
            return new RawCashgame(cashgame.Id, cashgame.BunchId, cashgame.LocationId, rawStatus, date);
        }

	    private static Cashgame CreateCashgame(RawCashgame rawGame)
	    {
            return new Cashgame(rawGame.BunchId, rawGame.LocationId, (GameStatus)rawGame.Status, rawGame.Id);
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
                var cashgame = CreateCashgame(rawGame);
                cashgame.AddCheckpoints(checkpoints);
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
            const string sql = "INSERT INTO cashgamecheckpoint (GameID, PlayerID, Type, Amount, Stack, Timestamp) VALUES (@gameId, @playerId, @type, @amount, @stack, @timestamp) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
	        var parameters = new List<SimpleSqlParameter>
	        {
	            new SimpleSqlParameter("@gameId", checkpoint.CashgameId),
	            new SimpleSqlParameter("@playerId", checkpoint.PlayerId),
	            new SimpleSqlParameter("@type", checkpoint.Type),
	            new SimpleSqlParameter("@amount", checkpoint.Amount),
	            new SimpleSqlParameter("@stack", checkpoint.Stack),
	            new SimpleSqlParameter("@timestamp", checkpoint.Timestamp.ToUniversalTime())
	        };
            return _db.ExecuteInsert(sql, parameters);
        }

	    private void UpdateCheckpoint(Checkpoint checkpoint)
        {
            const string sql = "UPDATE cashgamecheckpoint SET Timestamp = @timestamp, Amount = @amount, Stack = @stack WHERE CheckpointID = @checkpointId";
	        var parameters = new List<SimpleSqlParameter>
	        {
	            new SimpleSqlParameter("@timestamp", checkpoint.Timestamp),
	            new SimpleSqlParameter("@amount", checkpoint.Amount),
	            new SimpleSqlParameter("@stack", checkpoint.Stack),
	            new SimpleSqlParameter("@checkpointId", checkpoint.Id)
	        };
            _db.Execute(sql, parameters);
        }

	    private void DeleteCheckpoint(Checkpoint checkpoint)
        {
            const string sql = "DELETE FROM cashgamecheckpoint WHERE CheckpointID = @checkpointId";
	        var parameters = new List<SimpleSqlParameter>
	        {
	            new SimpleSqlParameter("@checkpointId", checkpoint.Id)
	        };
            _db.Execute(sql, parameters);
        }

 	    private IList<RawCheckpoint> GetCheckpoints(int cashgameId)
        {
            const string sql = "SELECT cp.GameID, cp.CheckpointID, cp.PlayerID, cp.Type, cp.Stack, cp.Amount, cp.Timestamp FROM cashgamecheckpoint cp WHERE cp.GameID = @cashgameId ORDER BY cp.PlayerID, cp.Timestamp";
 	        var parameters = new List<SimpleSqlParameter>
 	        {
 	            new SimpleSqlParameter("@cashgameId", cashgameId)
 	        };
            var reader = _db.Query(sql, parameters);
            return reader.ReadList(CreateRawCheckpoint);
        }

        private static RawCheckpoint CreateRawCheckpoint(IStorageDataReader reader)
        {
            return new RawCheckpoint(
                reader.GetIntValue("GameID"),
                reader.GetIntValue("PlayerID"),
                reader.GetIntValue("Amount"),
                reader.GetIntValue("Stack"),
                TimeZoneInfo.ConvertTimeToUtc(reader.GetDateTimeValue("TimeStamp")),
                reader.GetIntValue("CheckpointID"),
                reader.GetIntValue("Type"));
        }

        public IList<RawCheckpoint> GetCheckpoints(IList<int> cashgameIdList)
        {
            const string sql = "SELECT cp.GameID, cp.CheckpointID, cp.PlayerID, cp.Type, cp.Stack, cp.Amount, cp.Timestamp FROM cashgamecheckpoint cp WHERE cp.GameID IN (@cashgameIdList) ORDER BY cp.PlayerID, cp.Timestamp";
            var parameter = new ListSqlParameter("@cashgameIdList", cashgameIdList);
            var reader = _db.Query(sql, parameter);
            return reader.ReadList(CreateRawCheckpoint);
        }
	}
}