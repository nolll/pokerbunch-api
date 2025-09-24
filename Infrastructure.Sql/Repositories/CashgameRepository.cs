using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class CashgameRepository(IDb db, ICache cache) : ICashgameRepository
{
    private readonly CashgameDb _cashgameDb = new(db);

    public Task<Cashgame> Get(string cashgameId)
    {
        return cache.GetAndStoreAsync(_cashgameDb.Get, cashgameId, TimeSpan.FromMinutes(CacheTime.Long));
    }

    private Task<IList<Cashgame>> Get(IList<string> ids)
    {
        return cache.GetAndStoreAsync(_cashgameDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }
    
    public async Task<IList<Cashgame>> GetFinished(string slug, int? year = null)
    {
        var ids = year == null
            ? await _cashgameDb.FindFinished(slug)
            : await _cashgameDb.FindFinished(slug, year.Value);
        return await Get(ids);
    }

    public async Task<IList<Cashgame>> GetByEvent(string eventId)
    {
        var ids = await _cashgameDb.FindByEvent(eventId);
        return await Get(ids);
    }

    public async Task<IList<Cashgame>> GetByPlayer(string playerId)
    {
        var ids = await _cashgameDb.FindByPlayerId(playerId);
        return await Get(ids);
    }
    
    public async Task<Cashgame?> GetRunning(string slug)
    {
        var ids = await _cashgameDb.FindRunning(slug);
        return (await Get(ids)).FirstOrDefault();
    }

    public async Task<Cashgame> GetByCheckpoint(string checkpointId)
    {
        var ids = await _cashgameDb.FindByCheckpoint(checkpointId);
        return (await Get(ids)).First();
    }

    public async Task DeleteGame(string id)
    {
        await _cashgameDb.DeleteGame(id);
        cache.Remove<Cashgame>(id);
    }

    public Task<string> Add(Bunch bunch, Cashgame cashgame)
    {
        return _cashgameDb.AddGame(bunch, cashgame);
    }

    public async Task Update(Cashgame cashgame)
    {
        await _cashgameDb.UpdateGame(cashgame);
        cache.Remove<Cashgame>(cashgame.Id);
        if(cashgame.EventId is not null)
            cache.Remove<Event>(cashgame.EventId);
    }
}