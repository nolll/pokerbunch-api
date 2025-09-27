using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;
using SqlKata;

namespace Infrastructure.Sql.SqlDb;

public class PlayerDb(IDb db)
{
    private static Query PlayerQuery => new(Schema.Player);

    private static Query GetQuery => PlayerQuery
        .Select(
            Schema.Player.BunchId,
            Schema.Player.Id,
            Schema.Player.UserId,
            Schema.Player.RoleId,
            Schema.Player.Color,
            Schema.User.UserName,
            Schema.Bunch.Name)
        .SelectRaw($"COALESCE({Schema.User.DisplayName}, {Schema.Player.PlayerName}) AS {Schema.Player.PlayerName.AsParam()}")
        .SelectRaw($"{Schema.Bunch.Name} AS {Schema.Bunch.Slug.AsParam()}")
        .LeftJoin(Schema.User, Schema.User.Id, Schema.Player.UserId)
        .LeftJoin(Schema.Bunch, Schema.Bunch.Id, Schema.Player.BunchId);

    private static Query FindQuery => PlayerQuery
        .Select(Schema.Player.Id)
        .LeftJoin(Schema.Bunch, Schema.Bunch.Id, Schema.Player.BunchId);
    
    public async Task<IList<string>> Find(string slug)
    {
        var query = FindQuery.Where(Schema.Bunch.Name, slug);
        var result = await db.GetAsync<string>(query);
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByUser(string bunchId, string userId)
    {
        var query = FindQuery.Where(Schema.Player.BunchId, int.Parse(bunchId)).Where(Schema.Player.UserId, int.Parse(userId));
        var result = await db.GetAsync<string>(query);
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<Player>> Get(IList<string> ids)
    {
        if (!ids.Any())
            return new List<Player>();

        var query = GetQuery.WhereIn(Schema.Player.Id, ids.Select(int.Parse));
        var playerDtos = await db.GetAsync<PlayerDto>(query);

        return playerDtos.Select(PlayerMapper.ToPlayer).ToList();
    }

    public async Task<Player> Get(string id)
    {
        var query = GetQuery.Where(Schema.Player.Id, int.Parse(id));
        var playerDto = await db.FirstOrDefaultAsync<PlayerDto?>(query);
        var player = playerDto?.ToPlayer();

        if (player is null)
            throw new PokerBunchException($"Player with id {id} was not found");

        return player;
    }

    public async Task<string> Add(Player player) => player.IsUser 
        ? await AddWithUser(player) 
        : await AddWithoutUser(player);

    private async Task<string> AddWithUser(Player player)
    {
        var sql = $"""
                   INSERT INTO {Schema.Player} 
                   (
                     {Schema.Player.BunchId.AsParam()}, 
                     {Schema.Player.UserId.AsParam()},
                     {Schema.Player.RoleId.AsParam()},
                     {Schema.Player.Approved.AsParam()},
                     {Schema.Player.Color.AsParam()}
                   )
                   VALUES
                   (
                     (SELECT {Schema.Bunch.Id} FROM {Schema.Bunch} WHERE {Schema.Bunch.Name} = @{Schema.Bunch.Slug.AsParam()}), 
                     @{Schema.Player.UserId.AsParam()},
                     @{Schema.Player.RoleId.AsParam()},
                     @{Schema.Player.Approved.AsParam()},
                     @{Schema.Player.Color.AsParam()}
                   )
                   RETURNING {Schema.Player.Id.AsParam()}
                   """;

        var parameters = new Dictionary<string, object?>
        {
            { Schema.Bunch.Slug.AsParam(), player.BunchSlug },
            { Schema.Player.UserId.AsParam(), int.Parse(player.UserId!) },
            { Schema.Player.RoleId.AsParam(), (int)player.Role },
            { Schema.Player.Approved.AsParam(), true },
            { Schema.Player.Color.AsParam(), player.Color }
        };
        
        var result = await db.CustomInsert(sql, parameters);
        return result.ToString();
    }
    
    private async Task<string> AddWithoutUser(Player player)
    {
        var sql = $"""
                   INSERT INTO {Schema.Player} 
                   (
                     {Schema.Player.BunchId.AsParam()}, 
                     {Schema.Player.RoleId.AsParam()},
                     {Schema.Player.Approved.AsParam()},
                     {Schema.Player.PlayerName.AsParam()},
                     {Schema.Player.Color.AsParam()}
                   )
                   VALUES
                   (
                     (SELECT {Schema.Bunch.Id} FROM {Schema.Bunch} WHERE {Schema.Bunch.Name} = @{Schema.Bunch.Slug.AsParam()}), 
                     @{Schema.Player.RoleId.AsParam()},
                     @{Schema.Player.Approved.AsParam()},
                     @{Schema.Player.PlayerName.AsParam()},
                     @{Schema.Player.Color.AsParam()}
                   )
                   RETURNING {Schema.Player.Id.AsParam()}
                   """;

        var parameters = new Dictionary<string, object?>
        {
            { Schema.Bunch.Slug.AsParam(), player.BunchSlug },
            { Schema.Player.RoleId.AsParam(), (int)Role.Player },
            { Schema.Player.Approved.AsParam(), true },
            { Schema.Player.PlayerName.AsParam(), player.DisplayName },
            { Schema.Player.Color.AsParam(), player.Color }
        };
        
        var result = await db.CustomInsert(sql, parameters);
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
        var rowCount = await db.UpdateAsync(query, parameters);
        return rowCount > 0;
    }

    public async Task Delete(string playerId)
    {
        var query = PlayerQuery.Where(Schema.Player.Id, int.Parse(playerId));
        await db.DeleteAsync(query);
    }
}