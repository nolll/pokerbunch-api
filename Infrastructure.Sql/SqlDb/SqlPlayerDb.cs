using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Classes;
using Infrastructure.Sql.Interfaces;
using Infrastructure.Sql.SqlParameters;

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

    public async Task<IList<string>> Find(string bunchId)
    {
        var sql = string.Concat(SearchSql, "WHERE p.bunch_id = @bunchId");
        var parameters = new List<SqlParam>
        {
            new IntParam("@bunchId", bunchId)
        };
        var reader = await _db.QueryAsync(sql, parameters);
        return reader.ReadIntList("player_id").Select(o => o.ToString()).ToList();

    }

    public async Task<IList<string>> FindByUser(string bunchId, string userId)
    {
        var sql = string.Concat(SearchSql, "WHERE p.bunch_id = @bunchId AND p.user_id = @userId");
        var parameters = new List<SqlParam>
        {
            new IntParam("@bunchId", bunchId),
            new IntParam("@userId", userId)
        };
        var reader = await _db.QueryAsync(sql, parameters);
        return reader.ReadIntList("player_id").Select(o => o.ToString()).ToList();
    }

    public async Task<IList<Player>> Get(IList<string> ids)
    {
        if(!ids.Any())
            return new List<Player>();
        var sql = string.Concat(DataSql, "WHERE p.player_id IN (@ids)");
        var parameter = new IntListParam("@ids", ids);
        var reader = await _db.QueryAsync(sql, parameter);
        var rawPlayers = reader.ReadList(CreateRawPlayer);
        return rawPlayers.Select(CreatePlayer).ToList();
    }

    public async Task<Player> Get(string id)
    {
        var sql = string.Concat(DataSql, "WHERE p.player_id = @id");
        var parameters = new List<SqlParam>
        {
            new IntParam("@id", id)
        };
        var reader = await _db.QueryAsync(sql, parameters);
        var rawPlayer = reader.ReadOne(CreateRawPlayer);
        return rawPlayer != null ? CreatePlayer(rawPlayer) : null;
    }

    public async Task<string> Add(Player player)
    {
        if (player.IsUser)
        {
            const string sql = @"
                INSERT INTO pb_player (bunch_id, user_id, role_id, approved, color)
                VALUES (@bunchId, @userId, @role, @approved, @color) RETURNING player_id";
            var parameters = new List<SqlParam>
            {
                new IntParam("@bunchId", player.BunchId),
                new IntParam("@userId", player.UserId),
                new IntParam("@role", (int)player.Role),
                new BoolParam("@approved", true),
                new StringParam("@color", player.Color)
            };
            return (await _db.ExecuteInsertAsync(sql, parameters)).ToString();
        }
        else
        {
            const string sql = @"
                INSERT INTO pb_player (bunch_id, role_id, approved, player_name, color)
                VALUES (@bunchId, @role, @approved, @playerName, @color) RETURNING player_id";
            var parameters = new List<SqlParam>
            {
                new IntParam("@bunchId", player.BunchId),
                new IntParam("@role", (int)Role.Player),
                new BoolParam("@approved", true),
                new StringParam("@playerName", player.DisplayName),
                new StringParam("@color", player.Color)
            };
            return (await _db.ExecuteInsertAsync(sql, parameters)).ToString();
        }
    }

    public async Task<bool> JoinBunch(Player player, Bunch bunch, string userId)
    {
        const string sql = @"
            UPDATE pb_player
            SET bunch_id = @bunchId,
                player_name = NULL,
                user_id = @userId,
                role_id = @role,
                approved = @approved
            WHERE player_id = @playerId";
        var parameters = new List<SqlParam>
        {
            new IntParam("@bunchId", bunch.Id),
            new IntParam("@userId", userId),
            new IntParam("@role", (int)player.Role),
            new BoolParam("@approved", true),
            new IntParam("@playerId", player.Id)
        };
        var rowCount = await _db.ExecuteAsync(sql, parameters);
        return rowCount > 0;
    }

    public async Task Delete(string playerId)
    {
        const string sql = @"
            DELETE FROM pb_player
            WHERE player_id = @playerId";
        var parameters = new List<SqlParam>
        {
            new IntParam("@playerId", playerId)
        };
        await _db.ExecuteAsync(sql, parameters);
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
        var intUserId = reader.GetIntValue("user_id");
        var userId = intUserId != 0
            ? intUserId.ToString()
            : null;

        return new RawPlayer(
            reader.GetIntValue("bunch_id").ToString(),
            reader.GetIntValue("player_id").ToString(),
            userId,
            reader.GetStringValue("user_name"),
            reader.GetStringValue("player_name"),
            reader.GetIntValue("role_id"),
            reader.GetStringValue("color"));
    }
}