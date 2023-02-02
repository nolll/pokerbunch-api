using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;

namespace Infrastructure.Sql.SqlDb;

public class PlayerDb
{
    private readonly IDb _db;

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
        var param = new ListParam("@ids", ids.Select(int.Parse));
        var playerDtos = await _db.List<PlayerDto>(PlayerSql.GetByIdsQuery, param);
        return playerDtos.Select(PlayerMapper.ToPlayer).ToList();
    }

    public async Task<Player> Get(string id)
    {
        var @params = new
        {
            id = int.Parse(id)
        };

        var playerDto = await _db.Single<PlayerDto>(PlayerSql.GetByIdQuery, @params);
        return playerDto?.ToPlayer();
    }

    public async Task<string> Add(Player player)
    {
        if (player.IsUser)
        {
            var @params = new
            {
                bunchId = int.Parse(player.BunchId),
                userId = int.Parse(player.UserId),
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
            playerId = int.Parse(player.Id)
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