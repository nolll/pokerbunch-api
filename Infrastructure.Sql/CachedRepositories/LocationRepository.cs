using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.Repositories;

namespace Infrastructure.Sql.CachedRepositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly SqlLocationDb _locationDb;
        private readonly ICacheContainer _cacheContainer;

        public LocationRepository(SqlServerStorageProvider container, ICacheContainer cacheContainer)
        {
            _locationDb = new SqlLocationDb(container);
            _cacheContainer = cacheContainer;
        }

        public Location Get(int id)
        {
            return GetAndCache(id);
        }

        public IList<Location> List(IList<int> ids)
        {
            return GetAndCache(ids);
        }

        public IList<Location> List(int bunchId)
        {
            var ids = _locationDb.Find(bunchId);
            return GetAndCache(ids).OrderBy(o => o.Name).ToList();
        }

        public int Add(Location location)
        {
            return _locationDb.Add(location);
        }

        private Location GetAndCache(int id)
        {
            return _cacheContainer.GetAndStore(_locationDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
        }

        private IList<Location> GetAndCache(IList<int> ids)
        {
            return _cacheContainer.GetAndStore(_locationDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
        }
    }
}