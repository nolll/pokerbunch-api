using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class CashgameRepository : ICashgameRepository
{
    private readonly CashgameDb _cashgameDb;
    private readonly ICacheContainer _cacheContainer;

    public CashgameRepository(IDb db, ICacheContainer cacheContainer)
    {
        _cashgameDb = new CashgameDb(db);
        _cacheContainer = cacheContainer;
    }

    public async Task<Cashgame> Get(string cashgameId)
    {
        return await _cacheContainer.GetAndStoreAsync(_cashgameDb.Get, cashgameId, TimeSpan.FromMinutes(CacheTime.Long));
    }

    private async Task<IList<Cashgame>> Get(IList<string> ids)
    {
        return await _cacheContainer.GetAndStoreAsync(_cashgameDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }

    public async Task<IList<Cashgame>> GetFinished(string bunchId, int? year = null)
    {
        var ids = year == null
            ? await _cashgameDb.FindFinished(bunchId)
            : await _cashgameDb.FindFinished(bunchId, year.Value);
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

    public async Task<Cashgame> GetRunning(string bunchId)
    {
        var ids = await _cashgameDb.FindRunning(bunchId);
        return (await Get(ids)).FirstOrDefault();
    }

    public async Task<Cashgame> GetByCheckpoint(string checkpointId)
    {
        var ids = await _cashgameDb.FindByCheckpoint(checkpointId);
        return (await Get(ids)).FirstOrDefault();
    }

    public async Task DeleteGame(string id)
    {
        await _cashgameDb.DeleteGame(id);
        _cacheContainer.Remove<Cashgame>(id);
    }

    public async Task<string> Add(Bunch bunch, Cashgame cashgame)
    {
        return await _cashgameDb.AddGame(bunch, cashgame);
    }

    public async Task Update(Cashgame cashgame)
    {
        await _cashgameDb.UpdateGame(cashgame);
        _cacheContainer.Remove<Cashgame>(cashgame.Id);
    }
}