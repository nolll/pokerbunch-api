using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class LocationRepository(IDb container, ICache cache) : ILocationRepository
{
    private readonly LocationDb _locationDb = new(container);

    public Task<Location> Get(string id) => GetAndCache(id);

    public Task<IList<Location>> List(IList<string> ids) => GetAndCache(ids);

    public async Task<IList<Location>> List(string slug)
    {
        var ids = await _locationDb.Find(slug);
        var locations = await GetAndCache(ids);
        return locations.OrderBy(o => o.Name).ToList();
    }

    public Task<string> Add(Location location) => _locationDb.Add(location);

    private Task<Location> GetAndCache(string id) => 
        cache.GetAndStoreAsync(_locationDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));

    private Task<IList<Location>> GetAndCache(IList<string> ids) => 
        cache.GetAndStoreAsync(_locationDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
}