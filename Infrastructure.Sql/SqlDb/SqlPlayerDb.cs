using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Classes;
using Infrastructure.Sql.Interfaces;

namespace Infrastructure.Sql.SqlDb;

public class SqlPlayerDb
{
    private const string DataSql = @"
SELECT p.bunch_id, p.player_id, p.user_id, p.role_id, COALESCE(u.display_name, p.player_name) AS player_name, p.color, u.user_name 
FROM pb_player p 
LEFT JOIN pb_user u ON u.user_id = p.user_id ";
        
    private const string SearchSql = @"
SELECT p.player_id
FROM pb_player p ";

    private readonly PostgresStorageProvider _db;

    public SqlPlayerDb(PostgresStorageProvider db)
    {
        _db = db;
    }

    public IList<int> Find(int bunchId)
    {
        var sql = string.Concat(SearchSql, "WHERE p.bunch_id = @bunchId");
        var parameters = new List<SimpleSqlParameter>
        {
            new SimpleSqlParameter("@bunchId", bunchId)
        };
        var reader = _db.Query(sql, parameters);
        return reader.ReadIntList("player_id");

    }

    public IList<int> Find(int bunchId, string name)
    {
        var sql = string.Concat(SearchSql, "LEFT JOIN pb_user u on p.user_id = u.user_id WHERE p.bunch_id = @bunchId AND (p.player_name = @playerName OR u.display_name = @playerName)");
        var parameters = new List<SimpleSqlParameter>
        {
            new SimpleSqlParameter("@bunchId", bunchId),
            new SimpleSqlParameter("@playerName", name)
        };
        var reader = _db.Query(sql, parameters);
        return reader.ReadIntList("player_id");

    }

    public IList<int> Find(int bunchId, int userId)
    {
        var sql = string.Concat(SearchSql, "WHERE p.bunch_id = @bunchId AND p.user_id = @userId");
        var parameters = new List<SimpleSqlParameter>
        {
            new SimpleSqlParameter("@bunchId", bunchId),
            new SimpleSqlParameter("@userId", userId)
        };
        var reader = _db.Query(sql, parameters);
        return reader.ReadIntList("player_id");
    }

    public IList<Player> Get(IList<int> ids)
    {
        if(!ids.Any())
            return new List<Player>();
        var sql = string.Concat(DataSql, "WHERE p.player_id IN (@ids)");
        var parameter = new ListSqlParameter("@ids", ids);
        var reader = _db.Query(sql, parameter);
        var rawPlayers = reader.ReadList(CreateRawPlayer);
        return rawPlayers.Select(CreatePlayer).ToList();
    }

    public Player Get(int id)
    {
        var sql = string.Concat(DataSql, "WHERE p.player_id = @id");
        var parameters = new List<SimpleSqlParameter>
        {
            new SimpleSqlParameter("@id", id)
        };
        var reader = _db.Query(sql, parameters);
        var rawPlayer = reader.ReadOne(CreateRawPlayer);
        return rawPlayer != null ? CreatePlayer(rawPlayer) : null;
    }

    public int Add(Player player)
    {
        if (player.IsUser)
        {
            const string sql = @"
INSERT INTO pb_player (bunch_id, user_id, role_id, approved, color)
VALUES (@bunchId, @userId, @role, @approved, @color) RETURNING player_id";
            var parameters = new List<SimpleSqlParameter>
            {
                new SimpleSqlParameter("@bunchId", player.BunchId),
                new SimpleSqlParameter("@userId", player.UserId),
                new SimpleSqlParameter("@role", (int)player.Role),
                new SimpleSqlParameter("@approved", true),
                new SimpleSqlParameter("@color", player.Color)
            };
            return _db.ExecuteInsert(sql, parameters);
        }
        else
        {
            const string sql = @"
INSERT INTO pb_player (bunch_id, role_id, approved, player_name, color)
VALUES (@bunchId, @role, @approved, @playerName, @color) RETURNING player_id";
            var parameters = new List<SimpleSqlParameter>
            {
                new SimpleSqlParameter("@bunchId", player.BunchId),
                new SimpleSqlParameter("@role", (int)Role.Player),
                new SimpleSqlParameter("@approved", true),
                new SimpleSqlParameter("@playerName", player.DisplayName),
                new SimpleSqlParameter("@color", player.Color)
            };
            return _db.ExecuteInsert(sql, parameters);
        }
    }

    public bool JoinHomegame(Player player, Bunch bunch, int userId)
    {
        const string sql = @"
UPDATE player
SET bunch_id = @bunchId,
    player_name = NULL,
    user_id = @userId,
    role_id = @role,
    approved = @approved,
WHERE player_id = @playerId";
        var parameters = new List<SimpleSqlParameter>
        {
            new SimpleSqlParameter("@bunchId", bunch.Id),
            new SimpleSqlParameter("@userId", userId),
            new SimpleSqlParameter("@role", (int) player.Role),
            new SimpleSqlParameter("@approved", true),
            new SimpleSqlParameter("@playerId", player.Id)
        };
        var rowCount = _db.Execute(sql, parameters);
        return rowCount > 0;
    }

    public void Delete(int playerId)
    {
        const string sql = @"
DELETE FROM pb_player
WHERE player_id = @playerId";
        var parameters = new List<SimpleSqlParameter>
        {
            new SimpleSqlParameter("@playerId", playerId)
        };
        _db.Execute(sql, parameters);
    }

    private Player CreatePlayer(RawPlayer rawPlayer)
    {
        return new Player(
            rawPlayer.BunchId,
            rawPlayer.Id,
            rawPlayer.UserId,
            rawPlayer.UserName,
            rawPlayer.DisplayName,
            (Role)rawPlayer.Role,
            rawPlayer.Color);
    }

    private static RawPlayer CreateRawPlayer(IStorageDataReader reader)
    {
        return new RawPlayer(
            reader.GetIntValue("bunch_id"),
            reader.GetIntValue("player_id"),
            reader.GetIntValue("user_id"),
            reader.GetStringValue("user_name"),
            reader.GetStringValue("player_name"),
            reader.GetIntValue("role_id"),
            reader.GetStringValue("color"));
    }
}