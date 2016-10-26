using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.Repositories;

namespace Infrastructure.Sql.CachedRepositories
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
            return _cacheContainer.GetAndStore(_appDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<App> GetList(IList<int> ids)
        {
            return _cacheContainer.GetAndStore(_appDb.GetList, ids, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<int> Find()
        {
            return _appDb.Find();
        }

        public IList<int> Find(int userId)
        {
            return _appDb.Find(userId);
        }

        public IList<int> Find(string appKey)
        {
            return _appDb.Find(appKey);
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
    }
}