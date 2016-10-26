using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.Repositories;

namespace Infrastructure.Sql.CachedRepositories
{
    public class CashgameRepository : ICashgameRepository
    {
        private readonly SqlCashgameDb _cashgameDb;
        private readonly ICacheContainer _cacheContainer;

        public CashgameRepository(SqlServerStorageProvider db, ICacheContainer cacheContainer)
        {
            _cashgameDb = new SqlCashgameDb(db);
            _cacheContainer = cacheContainer;
        }

        public Cashgame Get(int cashgameId)
        {
            return _cacheContainer.GetAndStore(_cashgameDb.Get, cashgameId, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<Cashgame> Get(IList<int> ids)
        {
            return _cacheContainer.GetAndStore(_cashgameDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<int> FindFinished(int bunchId, int? year = null)
        {
            return _cashgameDb.FindFinished(bunchId, year);
        }

        public IList<int> FindByEvent(int eventId)
        {
            return _cashgameDb.FindByEvent(eventId);
        }

        public IList<int> FindByPlayerId(int playerId)
        {
            return _cashgameDb.FindByPlayerId(playerId);
        }

        public IList<int> FindRunning(int bunchId)
        {
            return _cashgameDb.FindRunning(bunchId);
        }

        public IList<int> FindByCheckpoint(int checkpointId)
        {
            return _cashgameDb.FindByCheckpoint(checkpointId);
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

        public int AddGame(Bunch bunch, Cashgame cashgame)
        {
            return _cashgameDb.AddGame(bunch, cashgame);
        }

        public void UpdateGame(Cashgame cashgame)
        {
            _cashgameDb.UpdateGame(cashgame);
            _cacheContainer.Remove<Cashgame>(cashgame.Id);
        }
    }
}