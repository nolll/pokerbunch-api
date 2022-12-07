using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Dtos;

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

    private readonly IDb _db;

    public SqlPlayerDb(IDb db)
    {
        _db = db;
    }

    public async Task<IList<string>> Find(string bunchId)
    {
        var sql = string.Concat(SearchSql, "WHERE p.bunch_id = @bunchId");

        var @params = new
        {
            bunchId = int.Parse(bunchId)
        };
        
        return (await _db.List<int>(sql, @params)).Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByUser(string bunchId, string userId)
    {
        var sql = string.Concat(SearchSql, "WHERE p.bunch_id = @bunchId AND p.user_id = @userId");
        
        var @params = new
        {
            bunchId = int.Parse(bunchId),
            userId = int.Parse(userId)
        };
        
        return (await _db.List<int>(sql, @params)).Select(o => o.ToString()).ToList();
    }

    public async Task<IList<Player>> Get(IList<string> ids)
    {
        if(!ids.Any())
            return new List<Player>();
        var sql = string.Concat(DataSql, "WHERE p.player_id IN (@ids)");
        var param = new ListParam("@ids", ids.Select(int.Parse));
        var rawPlayers = await _db.List<RawPlayer>(sql, param);
        return rawPlayers.Select(CreatePlayer).ToList();
    }

    public async Task<Player> Get(string id)
    {
        var sql = string.Concat(DataSql, "WHERE p.player_id = @id");

        var @params = new
        {
            id = int.Parse(id)
        };

        var rawPlayer = await _db.Single<RawPlayer>(sql, @params);
        return rawPlayer != null ? CreatePlayer(rawPlayer) : null;
    }

    public async Task<string> Add(Player player)
    {
        if (player.IsUser)
        {
            const string sql = @"
                INSERT INTO pb_player (bunch_id, user_id, role_id, approved, color)
                VALUES (@bunchId, @userId, @role, @approved, @color) RETURNING player_id";

            var @params = new
            {
                bunchId = int.Parse(player.BunchId),
                userId = int.Parse(player.UserId),
                role = (int)player.Role,
                approved = true,
                color = player.Color
            };

            return (await _db.Insert(sql, @params)).ToString();
        }
        else
        {
            const string sql = @"
                INSERT INTO pb_player (bunch_id, role_id, approved, player_name, color)
                VALUES (@bunchId, @role, @approved, @playerName, @color) RETURNING player_id";

            var @params = new
            {
                bunchId = int.Parse(player.BunchId),
                role = (int)Role.Player,
                approved = true,
                playerName = player.DisplayName,
                color = player.Color
            };

            return (await _db.Insert(sql, @params)).ToString();
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

        var @params = new
        {
            bunchId = int.Parse(bunch.Id),
            userId = int.Parse(userId),
            role = (int)player.Role,
            approved = true,
            playerId = int.Parse(player.Id)
        };

        var rowCount = await _db.Execute(sql, @params);
        return rowCount > 0;
    }

    public async Task Delete(string playerId)
    {
        const string sql = @"
            DELETE FROM pb_player
            WHERE player_id = @playerId";

        var @params = new
        {
            playerId = int.Parse(playerId)
        };

        await _db.Execute(sql, @params);
    }

    private Player CreatePlayer(RawPlayer rawPlayer)
    {
        return new Player(
            rawPlayer.Bunch_Id.ToString(),
            rawPlayer.Player_Id.ToString(),
            rawPlayer.User_Id != 0 ? rawPlayer.User_Id.ToString() : null,
            rawPlayer.User_Name,
            rawPlayer.Player_Name,
            (Role)rawPlayer.Role_Id,
            rawPlayer.Color);
    }
}