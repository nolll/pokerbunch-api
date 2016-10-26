using System;
using System.Collections.Generic;
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
            return _cacheContainer.GetAndStore(_locationDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<Location> Get(IList<int> ids)
        {
            return _cacheContainer.GetAndStore(_locationDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<int> Find(int bunchId)
        {
            return _locationDb.Find(bunchId);
        }

        public IList<int> Find(int bunchId, string name)
        {
            return _locationDb.Find(bunchId, name);
        }

        public int Add(Location location)
        {
            return _locationDb.Add(location);
        }
    }
}