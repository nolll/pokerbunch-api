using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class LocationRepository(IDb container, ICache cache) : ILocationRepository
{
    private readonly LocationDb _locationDb = new(container);

    public Task<Location> Get(string id)
    {
        return GetAndCache(id);
    }

    public Task<IList<Location>> List(IList<string> ids)
    {
        return GetAndCache(ids);
    }

    public async Task<IList<Location>> List(string bunchId)
    {
        var ids = await _locationDb.Find(bunchId);
        return (await GetAndCache(ids)).OrderBy(o => o.Name).ToList();
    }

    public Task<string> Add(Location location)
    {
        return _locationDb.Add(location);
    }

    private Task<Location> GetAndCache(string id)
    {
        return cache.GetAndStoreAsync(_locationDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
    }

    private Task<IList<Location>> GetAndCache(IList<string> ids)
    {
        return cache.GetAndStoreAsync(_locationDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }
}