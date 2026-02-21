using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.SqlDb;

public class JoinRequestDb(PokerBunchDbContext db)
{
    public async Task<IList<string>> Find(string slug)
    {
        var query = db.PbJoinRequest
            .Where(o => o.Bunch.Name == slug)
            .Select(o => o.JoinRequestId);

        var result = await query.ToListAsync();
        return result.Select(o => o.ToString()).ToList();
    }
    
    public async Task<IList<string>> Find(string bunchId, string userId)
    {
        var query = db.PbJoinRequest
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

        var query = db.PbJoinRequest
            .Where(o => ids.Select(int.Parse).Contains(o.JoinRequestId))
            .Select(o => new JoinRequestDto
            {
                Join_Request_Id = o.JoinRequestId,
                Bunch_Id = o.BunchId,
                User_Id = o.UserId,
                User_Name = o.User.UserName
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
        
        db.PbJoinRequest.Add(dto);

        await db.SaveChangesAsync();
        return dto.JoinRequestId.ToString();
    }
    
    public async Task Delete(string joinRequestId)
    {
        db.PbJoinRequest.Remove(new PbJoinRequest { JoinRequestId = int.Parse(joinRequestId) });
        await db.SaveChangesAsync();
    }

    private async Task<int> GetBunchId(string slug) => await db.PbBunch
        .Where(o => o.Name == slug)
        .Select(o => o.BunchId)
        .FirstOrDefaultAsync();
}