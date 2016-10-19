using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Infrastructure.Sql.CachedRepositories
{
    public class CachedCashgameRepository : ICashgameRepository
    {
        private readonly ICashgameRepository _cashgameRepository;
        private readonly ICacheContainer _cacheContainer;

        public CachedCashgameRepository(ICashgameRepository cashgameRepository, ICacheContainer cacheContainer)
        {
            _cashgameRepository = cashgameRepository;
            _cacheContainer = cacheContainer;
        }

        public Cashgame Get(int cashgameId)
        {
            return _cacheContainer.GetAndStore(_cashgameRepository.Get, cashgameId, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<Cashgame> Get(IList<int> ids)
        {
            return _cacheContainer.GetAndStore(_cashgameRepository.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<int> FindFinished(int bunchId, int? year = null)
        {
            return _cashgameRepository.FindFinished(bunchId, year);
        }

        public IList<int> FindByEvent(int eventId)
        {
            return _cashgameRepository.FindByEvent(eventId);
        }

        public IList<int> FindByPlayerId(int playerId)
        {
            return _cashgameRepository.FindByPlayerId(playerId);
        }

        public IList<int> FindRunning(int bunchId)
        {
            return _cashgameRepository.FindRunning(bunchId);
        }

        public IList<int> FindByCheckpoint(int checkpointId)
        {
            return _cashgameRepository.FindByCheckpoint(checkpointId);
        }

        public IList<int> GetYears(int bunchId)
        {
            return _cashgameRepository.GetYears(bunchId);
        }

        public void DeleteGame(int id)
        {
            _cashgameRepository.DeleteGame(id);
            _cacheContainer.Remove<Cashgame>(id);
        }

        public int AddGame(Bunch bunch, Cashgame cashgame)
        {
            return _cashgameRepository.AddGame(bunch, cashgame);
        }

        public void UpdateGame(Cashgame cashgame)
        {
            _cashgameRepository.UpdateGame(cashgame);
            _cacheContainer.Remove<Cashgame>(cashgame.Id);
        }
    }
}