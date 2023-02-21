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
            Schema.Player.BunchId.FullName,
            Schema.Player.Id.FullName,
            Schema.Player.UserId.FullName,
            Schema.Player.RoleId.FullName,
            Schema.Player.Color.FullName,
            Schema.User.UserName.FullName)
        .SelectRaw($"COALESCE({Schema.User.DisplayName.FullName}, {Schema.Player.PlayerName.FullName}) AS {Schema.Player.PlayerName}")
        .LeftJoin(Schema.User, Schema.User.Id.FullName, Schema.Player.UserId.FullName);

    private static Query FindQuery => PlayerQuery.Select(Schema.Player.Id);

    public PlayerDb(IDb db)
    {
        _db = db;
    }

    public async Task<IList<string>> Find(string bunchId)
    {
        var query = FindQuery.Where(Schema.Player.BunchId, int.Parse(bunchId));
        var result = await _db.QueryFactory.FromQuery(query).GetAsync<string>();
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByUser(string bunchId, string userId)
    {
        var query = FindQuery.Where(Schema.Player.BunchId, int.Parse(bunchId)).Where(Schema.Player.UserId, int.Parse(userId));
        var result = await _db.QueryFactory.FromQuery(query).GetAsync<string>();
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<Player>> Get(IList<string> ids)
    {
        if (!ids.Any())
            return new List<Player>();

        var query = GetQuery.WhereIn(Schema.Player.Id, ids.Select(int.Parse));
        var playerDtos = await _db.QueryFactory.FromQuery(query).GetAsync<PlayerDto>();

        return playerDtos.Select(PlayerMapper.ToPlayer).ToList();
    }

    public async Task<Player> Get(string id)
    {
        var query = GetQuery.Where(Schema.Player.Id, int.Parse(id));
        var playerDto = await _db.QueryFactory.FromQuery(query).FirstOrDefaultAsync<PlayerDto?>();
        var player = playerDto?.ToPlayer();

        if (player is null)
            throw new PokerBunchException($"Player with id {id} was not found");

        return player;
    }

    public async Task<string> Add(Player player)
    {
        var parameters = player.IsUser
            ? new Dictionary<string, object?>
            {
                { Schema.Player.BunchId, int.Parse(player.BunchId) },
                { Schema.Player.UserId, int.Parse(player.UserId!) },
                { Schema.Player.RoleId, (int)player.Role },
                { Schema.Player.Approved, true },
                { Schema.Player.Color, player.Color }
            }
            : new Dictionary<string, object?>
            {
                { Schema.Player.BunchId, int.Parse(player.BunchId) },
                { Schema.Player.RoleId, (int)Role.Player },
                { Schema.Player.Approved, true },
                { Schema.Player.PlayerName, player.DisplayName },
                { Schema.Player.Color, player.Color }
            };

        var result = await _db.QueryFactory.FromQuery(PlayerQuery).InsertGetIdAsync<int>(parameters);
        return result.ToString();
    }

    public async Task<bool> JoinBunch(Player player, Bunch bunch, string userId)
    {
        var parameters = new Dictionary<string, object>
        {
            { Schema.Player.BunchId, int.Parse(bunch.Id) },
            { Schema.Player.UserId, int.Parse(userId) },
            { Schema.Player.RoleId, (int)player.Role },
            { Schema.Player.Approved, true }
        };

        var query = PlayerQuery.Where(Schema.Player.Id, int.Parse(player.Id));
        var rowCount = await _db.QueryFactory.FromQuery(query).UpdateAsync(parameters);
        return rowCount > 0;
    }

    public async Task Delete(string playerId)
    {
        var query = PlayerQuery.Where(Schema.Player.Id, int.Parse(playerId));
        await _db.QueryFactory.FromQuery(query).DeleteAsync();
    }
}