using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.Repositories;

namespace Infrastructure.Sql.CachedRepositories
{
    public class EventRepository : IEventRepository
    {
        private readonly SqlEventDb _eventDb;
        private readonly ICacheContainer _cacheContainer;

        public EventRepository(SqlServerStorageProvider db, ICacheContainer cacheContainer)
        {
            _eventDb = new SqlEventDb(db);
            _cacheContainer = cacheContainer;
        }

        public Event Get(int id)
        {
            return _cacheContainer.GetAndStore(_eventDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<Event> Get(IList<int> ids)
        {
            return _cacheContainer.GetAndStore(_eventDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<int> FindByBunchId(int bunchId)
        {
            return _eventDb.FindByBunchId(bunchId);
        }

        public IList<int> FindByCashgameId(int cashgameId)
        {
            return _eventDb.FindByBunchId(cashgameId);
        }

        public int Add(Event e)
        {
            return _eventDb.Add(e);
        }

        public void AddCashgame(int eventId, int cashgameId)
        {
            _eventDb.AddCashgame(eventId, cashgameId);
        }
    }
}