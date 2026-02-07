using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class JoinRequestRepository(IDb db, ICache cache) : IJoinRequestRepository
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
    
    public Task<IList<JoinRequest>> Get(IList<string> ids)
    {
        return cache.GetAndStoreAsync(_joinRequestDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }
}