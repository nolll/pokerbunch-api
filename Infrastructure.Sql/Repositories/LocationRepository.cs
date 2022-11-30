using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly SqlLocationDb _locationDb;
    private readonly ICacheContainer _cacheContainer;

    public LocationRepository(IDb container, ICacheContainer cacheContainer)
    {
        _locationDb = new SqlLocationDb(container);
        _cacheContainer = cacheContainer;
    }

    public async Task<Location> Get(string id)
    {
        return await GetAndCache(id);
    }

    public async Task<IList<Location>> List(IList<string> ids)
    {
        return await GetAndCache(ids);
    }

    public async Task<IList<Location>> List(string bunchId)
    {
        var ids = await _locationDb.Find(bunchId);
        return (await GetAndCache(ids)).OrderBy(o => o.Name).ToList();
    }

    public async Task<string> Add(Location location)
    {
        return await _locationDb.Add(location);
    }

    private async Task<Location> GetAndCache(string id)
    {
        return await _cacheContainer.GetAndStoreAsync(_locationDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
    }

    private async Task<IList<Location>> GetAndCache(IList<string> ids)
    {
        return await _cacheContainer.GetAndStoreAsync(_locationDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }
}