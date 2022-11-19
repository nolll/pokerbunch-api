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

    public Cashgame Get(int cashgameId)
    {
        return _cacheContainer.GetAndStore(_cashgameDb.Get, cashgameId, TimeSpan.FromMinutes(CacheTime.Long));
    }

    private IList<Cashgame> Get(IList<int> ids)
    {
        return _cacheContainer.GetAndStore(_cashgameDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }

    public IList<Cashgame> GetFinished(int bunchId, int? year = null)
    {
        var ids = _cashgameDb.FindFinished(bunchId, year);
        return Get(ids);
    }

    public IList<Cashgame> GetByEvent(int eventId)
    {
        var ids = _cashgameDb.FindByEvent(eventId);
        return Get(ids);
    }

    public IList<Cashgame> GetByPlayer(int playerId)
    {
        var ids = _cashgameDb.FindByPlayerId(playerId);
        return Get(ids);
    }

    public Cashgame GetRunning(int bunchId)
    {
        var ids = _cashgameDb.FindRunning(bunchId);
        return Get(ids).FirstOrDefault();
    }

    public Cashgame GetByCheckpoint(int checkpointId)
    {
        var ids = _cashgameDb.FindByCheckpoint(checkpointId);
        return Get(ids).FirstOrDefault();
    }

    public IList<int> GetYears(int bunchId)
    {
        return _cashgameDb.GetYears(bunchId);
    }

    public void DeleteGame(int id)
    {
        _cashgameDb.DeleteGame(id);
        _cacheContainer.Remove<Cashgame>(id);
    }

    public int Add(Bunch bunch, Cashgame cashgame)
    {
        return _cashgameDb.AddGame(bunch, cashgame);
    }

    public void Update(Cashgame cashgame)
    {
        _cashgameDb.UpdateGame(cashgame);
        _cacheContainer.Remove<Cashgame>(cashgame.Id);
    }
}