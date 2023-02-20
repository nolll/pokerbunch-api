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

    private static Query TableQuery => new(Schema.Player);

    private static Query GetQuery => TableQuery
        .Select(
            Schema.Player.BunchId.FullName,
            Schema.Player.Id.FullName,
            Schema.Player.UserId.FullName,
            Schema.Player.RoleId.FullName,
            Schema.Player.Color.FullName,
            Schema.User.UserName.FullName
        )
        .SelectRaw($"COALESCE({Schema.User.DisplayName.FullName}, {Schema.Player.PlayerName.FullName}) AS {Schema.Player.PlayerName}")
        .LeftJoin(Schema.User, Schema.User.Id.FullName, Schema.Player.UserId.FullName);

    private static Query FindQuery => TableQuery.Select(Schema.Player.Id);
    
    public PlayerDb(IDb db)
    {
        _db = db;
    }

    public async Task<IList<string>> Find(string bunchId)
    {
        var @params = new
        {
            bunchId = int.Parse(bunchId)
        };

        return (await _db.List<int>(PlayerSql.FindByBunchQuery, @params)).Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByUser(string bunchId, string userId)
    {
        var @params = new
        {
            bunchId = int.Parse(bunchId),
            userId = int.Parse(userId)
        };
        
        return (await _db.List<int>(PlayerSql.FindByUserQuery, @params)).Select(o => o.ToString()).ToList();
    }

    public async Task<IList<Player>> Get(IList<string> ids)
    {
        if(!ids.Any())
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
        if (player.IsUser)
        {
            var @params = new
            {
                bunchId = int.Parse(player.BunchId),
                userId = int.Parse(player.UserId!),
                role = (int)player.Role,
                approved = true,
                color = player.Color
            };

            return (await _db.Insert(PlayerSql.AddWithUserQuery, @params)).ToString();
        }
        else
        {
            var @params = new
            {
                bunchId = int.Parse(player.BunchId),
                role = (int)Role.Player,
                approved = true,
                playerName = player.DisplayName,
                color = player.Color
            };

            return (await _db.Insert(PlayerSql.AddQuery, @params)).ToString();
        }
    }

    public async Task<bool> JoinBunch(Player player, Bunch bunch, string userId)
    {
        var @params = new
        {
            bunchId = int.Parse(bunch.Id),
            userId = int.Parse(userId),
            role = (int)player.Role,
            approved = true,
            playerId = int.Parse(player.Id!)
        };

        var rowCount = await _db.Execute(PlayerSql.UpdateQuery, @params);
        return rowCount > 0;
    }

    public async Task Delete(string playerId)
    {
        var @params = new
        {
            playerId = int.Parse(playerId)
        };

        await _db.Execute(PlayerSql.DeleteQuery, @params);
    }
}