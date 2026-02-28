using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.Models;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class JoinRequestRepository(PokerBunchDbContext db, ICache cache) : IJoinRequestRepository
{
    private readonly JoinRequestDb _joinRequestDb = new(db);
    
    public Task<string> Add(JoinRequest joinRequest)
    {
        return _joinRequestDb.Add(joinRequest);
    }

    public async Task<IList<JoinRequest>> List(string slug)
    {
        var ids = await _joinRequestDb.Find(slug);
        return await Get(ids);
    }

    public async Task<JoinRequest?> Get(string bunchId, string userId)
    {
        var ids = await _joinRequestDb.Find(bunchId, userId);
        return (await Get(ids)).FirstOrDefault();
    }

    public Task<IList<JoinRequest>> Get(IList<string> ids) => 
        cache.GetAndStoreAsync(_joinRequestDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    
    public async Task Delete(string id)
    {
        await _joinRequestDb.Delete(id);
        cache.Remove<JoinRequest>(id);
    }
}