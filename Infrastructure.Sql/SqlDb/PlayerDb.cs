using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.SqlDb;

public class PlayerDb(PokerBunchDbContext db) : BaseDb(db)
{
    private readonly PokerBunchDbContext _db = db;

    public async Task<IList<string>> Find(string slug)
    {
        var q = _db.PbPlayer
            .Where(o => o.Bunch.Name == slug)
            .Select(o => o.PlayerId);

        var ids = await q.ToListAsync();
        return ids.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByUser(string bunchId, string userId)
    {
        var q = _db.PbPlayer
            .Where(o => o.BunchId == int.Parse(bunchId) && o.UserId == int.Parse(userId))
            .Select(o => o.PlayerId);

        var ids = await q.ToListAsync();
        return ids.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<Player>> Get(IList<string> ids)
    {
        if (!ids.Any())
            return new List<Player>();

        var q = _db.PbPlayer
            .Include(o => o.User)
            .Where(o => ids.Select(int.Parse).Contains(o.PlayerId))
            .Select(o => new PlayerDto
            {
                Player_Id = o.PlayerId,
                User_Id = o.UserId,
                Role_Id = o.RoleId,
                Color = o.Color,
                Player_Name = o.User != null ? o.User.DisplayName : o.PlayerName ?? ""
            });

        var dtos = await q.ToListAsync();
        return dtos.Select(PlayerMapper.ToPlayer).ToList();
    }

    public async Task<Player> Get(string id)
    {
        var players = await Get([id]);
        
        return players.Count != 0 
            ? players.First() 
            : throw new PokerBunchException($"Player with id {id} was not found");
    }

    public async Task<string> Add(Player player) => player.IsUser
        ? await AddWithUser(player)
        : await AddWithoutUser(player);

    private async Task<string> AddWithUser(Player player)
    {
        var bunchId = await GetBunchId(player.BunchSlug);
        var dto = new PbPlayer
        {
            BunchId = bunchId,
            UserId = int.Parse(player.UserId!),
            RoleId = (int)Role.Player,
            Approved = true,
            Color = player.Color
        };

        _db.PbPlayer.Add(dto);
        await _db.SaveChangesAsync();
        return dto.PlayerId.ToString();
    }

    private async Task<string> AddWithoutUser(Player player)
    {
        var bunchId = await GetBunchId(player.BunchSlug);
        var dto = new PbPlayer
        {
            BunchId = bunchId,
            RoleId = (int)Role.Player,
            Approved = true,
            PlayerName = player.DisplayName,
            Color = player.Color
        };

        _db.PbPlayer.Add(dto);
        await _db.SaveChangesAsync();
        return dto.PlayerId.ToString();
    }
    
    public async Task Delete(string playerId)
    {
        var dto = new PbPlayer { PlayerId = int.Parse(playerId) };
        _db.PbPlayer.Remove(dto);
        await _db.SaveChangesAsync();
    }
}