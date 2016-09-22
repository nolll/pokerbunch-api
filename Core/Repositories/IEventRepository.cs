using System.Collections.Generic;
using Core.Entities;

namespace Core.Repositories
{
    public interface IEventRepository
    {
        Event Get(int id);
        IList<Event> Get(IList<int> ids);
        IList<int> FindByBunchId(int bunchId);
        IList<int> FindByCashgameId(int cashgameId);
        int Add(Event e);
        void AddCashgame(int eventId, int cashgameId);
    }
}