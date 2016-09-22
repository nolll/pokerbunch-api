using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;

namespace Core.Services
{
    public class EventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public IList<Event> GetByBunch(int bunchId)
        {
            var ids = _eventRepository.FindByBunchId(bunchId);
            return _eventRepository.Get(ids);
        }

        public Event GetByCashgame(int bunchId)
        {
            var ids = _eventRepository.FindByCashgameId(bunchId);
            return _eventRepository.Get(ids).FirstOrDefault();
        }

        public Event Get(int eventId)
        {
            return _eventRepository.Get(eventId);
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