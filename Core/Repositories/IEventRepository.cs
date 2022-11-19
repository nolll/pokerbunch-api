using Core.Entities;

namespace Core.Repositories;

public interface IEventRepository
{
    Task<Event> Get(int id);
    Task<IList<Event>> Get(IList<int> ids);
    Task<IList<Event>> List(int bunchId);
    Task<Event> GetByCashgame(int cashgameId);
    Task<int> Add(Event e);
    Task AddCashgame(int eventId, int cashgameId);
}