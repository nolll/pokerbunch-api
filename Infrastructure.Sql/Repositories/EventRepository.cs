using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories
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

        public IList<Event> List(int bunchId)
        {
            var ids = _eventDb.FindByBunchId(bunchId);
            return Get(ids);
        }

        public Event GetByCashgame(int cashgameId)
        {
            var ids = _eventDb.FindByBunchId(cashgameId);
            return Get(ids).FirstOrDefault();
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