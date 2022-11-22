using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class CashgameRepository : ICashgameRepository
{
    private readonly SqlCashgameDb _cashgameDb;
    private readonly ICacheContainer _cacheContainer;

    public CashgameRepository(PostgresStorageProvider db, ICacheContainer cacheContainer)
    {
        _cashgameDb = new SqlCashgameDb(db);
        _cacheContainer = cacheContainer;
    }

    public async Task<Cashgame> Get(int cashgameId)
    {
        return await _cacheContainer.GetAndStoreAsync(_cashgameDb.Get, cashgameId, TimeSpan.FromMinutes(CacheTime.Long));
    }

    private async Task<IList<Cashgame>> Get(IList<int> ids)
    {
        return await _cacheContainer.GetAndStoreAsync(_cashgameDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }

    public async Task<IList<Cashgame>> GetFinished(int bunchId, int? year = null)
    {
        var ids = await _cashgameDb.FindFinished(bunchId, year);
        return await Get(ids);
    }

    public async Task<IList<Cashgame>> GetByEvent(int eventId)
    {
        var ids = await _cashgameDb.FindByEvent(eventId);
        return await Get(ids);
    }

    public async Task<IList<Cashgame>> GetByPlayer(int playerId)
    {
        var ids = await _cashgameDb.FindByPlayerId(playerId);
        return await Get(ids);
    }

    public async Task<Cashgame> GetRunning(int bunchId)
    {
        var ids = await _cashgameDb.FindRunning(bunchId);
        return (await Get(ids)).FirstOrDefault();
    }

    public async Task<Cashgame> GetByCheckpoint(int checkpointId)
    {
        var ids = await _cashgameDb.FindByCheckpoint(checkpointId);
        return (await Get(ids)).FirstOrDefault();
    }

    public async Task DeleteGame(int id)
    {
        await _cashgameDb.DeleteGame(id);
        _cacheContainer.Remove<Cashgame>(id);
    }

    public async Task<int> Add(Bunch bunch, Cashgame cashgame)
    {
        return await _cashgameDb.AddGame(bunch, cashgame);
    }

    public async Task Update(Cashgame cashgame)
    {
        await _cashgameDb.UpdateGame(cashgame);
        _cacheContainer.Remove<Cashgame>(cashgame.Id);
    }
}