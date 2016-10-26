using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.Repositories;

namespace Infrastructure.Sql.CachedRepositories
{
    public class BunchRepository : IBunchRepository
    {
        private readonly SqlBunchDb _bunchDb;
        private readonly ICacheContainer _cacheContainer;

        public BunchRepository(SqlServerStorageProvider db, ICacheContainer cacheContainer)
        {
            _bunchDb = new SqlBunchDb(db);
            _cacheContainer = cacheContainer;
        }

        public Bunch Get(int id)
        {
            return _cacheContainer.GetAndStore(_bunchDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<Bunch> Get(IList<int> ids)
        {
            return _cacheContainer.GetAndStore(_bunchDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<int> Search()
        {
            return _bunchDb.Search();
        }

        public IList<int> Search(string slug)
        {
            return _bunchDb.Search(slug);
        }

        public IList<int> Search(int userId)
        {
            return _bunchDb.Search(userId);
        }

        public int Add(Bunch bunch)
        {
            return _bunchDb.Add(bunch);
        }

        public void Update(Bunch bunch)
        {
            _bunchDb.Update(bunch);
            _cacheContainer.Remove<Bunch>(bunch.Id);
        }
    }
}