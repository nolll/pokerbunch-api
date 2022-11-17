using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly SqlLocationDb _locationDb;
    private readonly ICacheContainer _cacheContainer;

    public LocationRepository(PostgresStorageProvider container, ICacheContainer cacheContainer)
    {
        _locationDb = new SqlLocationDb(container);
        _cacheContainer = cacheContainer;
    }

    public async Task<Location> Get(int id)
    {
        return await GetAndCache(id);
    }

    public async Task<IList<Location>> List(IList<int> ids)
    {
        return await GetAndCache(ids);
    }

    public async Task<IList<Location>> List(int bunchId)
    {
        var ids = await _locationDb.Find(bunchId);
        return (await GetAndCache(ids)).OrderBy(o => o.Name).ToList();
    }

    public async Task<int> Add(Location location)
    {
        return await _locationDb.Add(location);
    }

    private async Task<Location> GetAndCache(int id)
    {
        return await _cacheContainer.GetAndStoreAsync(_locationDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
    }

    private async Task<IList<Location>> GetAndCache(IList<int> ids)
    {
        return await _cacheContainer.GetAndStoreAsync(_locationDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }
}