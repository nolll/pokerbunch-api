using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Infrastructure.Sql.CachedRepositories
{
    public class CachedEventRepository : IEventRepository
    {
        private readonly IEventRepository _eventRepository;
        private readonly ICacheContainer _cacheContainer;

        public CachedEventRepository(IEventRepository eventRepository, ICacheContainer cacheContainer)
        {
            _eventRepository = eventRepository;
            _cacheContainer = cacheContainer;
        }

        public Event Get(int id)
        {
            return _cacheContainer.GetAndStore(_eventRepository.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<Event> Get(IList<int> ids)
        {
            return _cacheContainer.GetAndStore(_eventRepository.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<int> FindByBunchId(int bunchId)
        {
            return _eventRepository.FindByBunchId(bunchId);
        }

        public IList<int> FindByCashgameId(int cashgameId)
        {
            return _eventRepository.FindByBunchId(cashgameId);
        }

        public int Add(Event e)
        {
            return _eventRepository.Add(e);
        }

        public void AddCashgame(int eventId, int cashgameId)
        {
            _eventRepository.AddCashgame(eventId, cashgameId);
        }
    }
}