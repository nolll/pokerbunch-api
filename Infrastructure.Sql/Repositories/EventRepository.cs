using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class EventRepository : IEventRepository
{
    private readonly EventDb _eventDb;
    private readonly ICache _cache;

    public EventRepository(IDb db, ICache cache)
    {
        _eventDb = new EventDb(db);
        _cache = cache;
    }

    public async Task<Event> Get(string id)
    {
        return await _cache.GetAndStoreAsync(_eventDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
    }

    public async Task<IList<Event>> Get(IList<string> ids)
    {
        return await _cache.GetAndStoreAsync(_eventDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }

    public async Task<IList<Event>> List(string bunchId)
    {
        var ids = await _eventDb.FindByBunchId(bunchId);
        return await Get(ids);
    }

    public async Task<Event?> GetByCashgame(string cashgameId)
    {
        var ids = await _eventDb.FindByCashgameId(cashgameId);
        return (await Get(ids)).FirstOrDefault();
    }

    public async Task<string> Add(Event e)
    {
        return await _eventDb.Add(e);
    }

    public async Task AddCashgame(string eventId, string cashgameId)
    {
        await _eventDb.AddCashgame(eventId, cashgameId);
    }

    public async Task RemoveCashgame(string eventId, string cashgameId)
    {
        await _eventDb.RemoveCashgame(eventId, cashgameId);
    }
}