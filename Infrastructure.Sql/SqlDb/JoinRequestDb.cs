using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.SqlDb;

public class JoinRequestDb(PokerBunchDbContext db) : BaseDb(db)
{
    private readonly PokerBunchDbContext _db = db;

    public async Task<IList<string>> Find(string slug)
    {
        var query = _db.PbJoinRequest
            .Where(o => o.Bunch.Name == slug)
            .Select(o => o.JoinRequestId);

        var result = await query.ToListAsync();
        return result.Select(o => o.ToString()).ToList();
    }
    
    public async Task<IList<string>> Find(string bunchId, string userId)
    {
        var query = _db.PbJoinRequest
            .Where(o => o.Bunch.BunchId == int.Parse(bunchId))
            .Where(o => o.User.UserId == int.Parse(userId))
            .Select(o => o.JoinRequestId);

        var result = await query.ToListAsync();
        return result.Select(o => o.ToString()).ToList();
    }
    
    public async Task<IList<JoinRequest>> Get(IList<string> ids)
    {
        if (!ids.Any())
            return [];

        var query = _db.PbJoinRequest
            .Where(o => ids.Select(int.Parse).Contains(o.JoinRequestId))
            .Select(o => new JoinRequestDto
            {
                JoinRequestId = o.JoinRequestId,
                BunchId = o.BunchId,
                UserId = o.UserId,
                UserName = o.User.UserName
            });

        var result = await query.ToListAsync();

        return result.Select(JoinRequestMapper.ToJoinRequest).ToList();
    }
    
    public async Task<string> Add(JoinRequest joinRequest)
    {
        var bunchId = await GetBunchId(joinRequest.BunchId);
        
        var dto = new PbJoinRequest
        {
            BunchId = bunchId,
            UserId = int.Parse(joinRequest.UserId)
        };
        
        _db.PbJoinRequest.Add(dto);

        await _db.SaveChangesAsync();
        return dto.JoinRequestId.ToString();
    }
    
    public async Task Delete(string joinRequestId)
    {
        _db.PbJoinRequest.Remove(new PbJoinRequest { JoinRequestId = int.Parse(joinRequestId) });
        await _db.SaveChangesAsync();
    }
}