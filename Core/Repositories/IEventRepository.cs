using Core.Entities;

namespace Core.Repositories;

public interface IEventRepository
{
    Task<Event> Get(string id);
    Task<IList<Event>> Get(IList<string> ids);
    Task<IList<Event>> List(string bunchId);
    Task<Event> GetByCashgame(string cashgameId);
    Task<string> Add(Event e);
    Task AddCashgame(string eventId, string cashgameId);
}