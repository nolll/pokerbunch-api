using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.Models;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class EventRepository(PokerBunchDbContext db, ICache cache) : IEventRepository
{
    private readonly EventDb _eventDb = new(db);

    public Task<Event> Get(string id)
    {
        return cache.GetAndStoreAsync(_eventDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
    }

    public Task<IList<Event>> Get(IList<string> ids)
    {
        return cache.GetAndStoreAsync(_eventDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }
    
    public async Task<IList<Event>> List(string bunchId)
    {
        var ids = await _eventDb.FindBySlug(bunchId);
        return await Get(ids);
    }

    public async Task<Event?> GetByCashgame(string cashgameId)
    {
        var ids = await _eventDb.FindByCashgameId(cashgameId);
        return (await Get(ids)).FirstOrDefault();
    }

    public Task<string> Add(Event e)
    {
        return _eventDb.Add(e);
    }

    public Task AddCashgame(string eventId, string cashgameId)
    {
        return _eventDb.AddCashgame(eventId, cashgameId);
    }

    public Task RemoveCashgame(string eventId, string cashgameId)
    {
        return _eventDb.RemoveCashgame(eventId, cashgameId);
    }
}