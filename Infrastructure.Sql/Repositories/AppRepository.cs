using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories
{
    public class AppRepository : IAppRepository
    {
        private readonly SqlAppDb _appDb;
        private readonly ICacheContainer _cacheContainer;

        public AppRepository(SqlServerStorageProvider db, ICacheContainer cacheContainer)
        {
            _appDb = new SqlAppDb(db);
            _cacheContainer = cacheContainer;
        }
        
        public App Get(int id)
        {
            return GetAndCache(id);
        }

        public IList<App> List()
        {
            var ids = _appDb.Find();
            return GetAndCache(ids);
        }

        public IList<App> List(int userId)
        {
            var ids = _appDb.Find(userId);
            return GetAndCache(ids);
        }

        public App Get(string appKey)
        {
            var ids = _appDb.Find(appKey);
            if (ids.Any())
                return GetAndCache(ids.First());
            throw new AppNotFoundException();
        }

        public int Add(App app)
        {
            return _appDb.Add(app);
        }

        public void Update(App app)
        {
            _appDb.Update(app);
            _cacheContainer.Remove<App>(app.Id);
        }

        private App GetAndCache(int id)
        {
            return _cacheContainer.GetAndStore(_appDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
        }

        private IList<App> GetAndCache(IList<int> ids)
        {
            return _cacheContainer.GetAndStore(_appDb.GetList, ids, TimeSpan.FromMinutes(CacheTime.Long));
        }
    }
}