using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;
using SqlKata;
using SqlKata.Execution;

namespace Infrastructure.Sql.SqlDb;

public class PlayerDb
{
    private readonly IDb _db;

    private static Query PlayerQuery => new(Schema.Player);

    private static Query GetQuery => PlayerQuery
        .Select(
            Schema.Player.BunchId,
            Schema.Player.Id,
            Schema.Player.UserId,
            Schema.Player.RoleId,
            Schema.Player.Color,
            Schema.User.UserName)
        .SelectRaw($"COALESCE({Schema.User.DisplayName}, {Schema.Player.PlayerName}) AS {Schema.Player.PlayerName.AsParam()}")
        .LeftJoin(Schema.User, Schema.User.Id, Schema.Player.UserId);

    private static Query FindQuery => PlayerQuery.Select(Schema.Player.Id);

    public PlayerDb(IDb db)
    {
        _db = db;
    }

    public async Task<IList<string>> Find(string bunchId)
    {
        var query = FindQuery.Where(Schema.Player.BunchId, int.Parse(bunchId));
        var result = await _db.GetAsync<string>(query);
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByUser(string bunchId, string userId)
    {
        var query = FindQuery.Where(Schema.Player.BunchId, int.Parse(bunchId)).Where(Schema.Player.UserId, int.Parse(userId));
        var result = await _db.GetAsync<string>(query);
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<Player>> Get(IList<string> ids)
    {
        if (!ids.Any())
            return new List<Player>();

        var query = GetQuery.WhereIn(Schema.Player.Id, ids.Select(int.Parse));
        var playerDtos = await _db.GetAsync<PlayerDto>(query);

        return playerDtos.Select(PlayerMapper.ToPlayer).ToList();
    }

    public async Task<Player> Get(string id)
    {
        var query = GetQuery.Where(Schema.Player.Id, int.Parse(id));
        var playerDto = await _db.FirstOrDefaultAsync<PlayerDto?>(query);
        var player = playerDto?.ToPlayer();

        if (player is null)
            throw new PokerBunchException($"Player with id {id} was not found");

        return player;
    }

    public async Task<string> Add(Player player)
    {
        var parameters = player.IsUser
            ? new Dictionary<SqlColumn, object?>
            {
                { Schema.Player.BunchId, int.Parse(player.BunchId) },
                { Schema.Player.UserId, int.Parse(player.UserId!) },
                { Schema.Player.RoleId, (int)player.Role },
                { Schema.Player.Approved, true },
                { Schema.Player.Color, player.Color }
            }
            : new Dictionary<SqlColumn, object?>
            {
                { Schema.Player.BunchId, int.Parse(player.BunchId) },
                { Schema.Player.RoleId, (int)Role.Player },
                { Schema.Player.Approved, true },
                { Schema.Player.PlayerName, player.DisplayName },
                { Schema.Player.Color, player.Color }
            };

        var result = await _db.InsertGetIdAsync(PlayerQuery, parameters);
        return result.ToString();
    }

    public async Task<bool> JoinBunch(Player player, Bunch bunch, string userId)
    {
        var parameters = new Dictionary<SqlColumn, object?>
        {
            { Schema.Player.BunchId, int.Parse(bunch.Id) },
            { Schema.Player.UserId, int.Parse(userId) },
            { Schema.Player.RoleId, (int)player.Role },
            { Schema.Player.Approved, true }
        };

        var query = PlayerQuery.Where(Schema.Player.Id, int.Parse(player.Id));
        var rowCount = await _db.UpdateAsync(query, parameters);
        return rowCount > 0;
    }

    public async Task Delete(string playerId)
    {
        var query = PlayerQuery.Where(Schema.Player.Id, int.Parse(playerId));
        await _db.DeleteAsync(query);
    }
}