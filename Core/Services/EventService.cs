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

        public Event Get(int eventId)
        {
            return _eventRepository.Get(eventId);
        }

        public IList<Event> GetByBunch(int bunchId)
        {
            return _eventRepository.GetByBunchId(bunchId);
        }

        public Event GetByCashgame(int cashgameId)
        {
            return _eventRepository.GetByCashgameId(cashgameId);
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