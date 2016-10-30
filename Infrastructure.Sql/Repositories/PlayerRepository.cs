using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly SqlPlayerDb _playerDb;
        private readonly ICacheContainer _cacheContainer;

        public PlayerRepository(SqlServerStorageProvider db, ICacheContainer cacheContainer)
        {
            _playerDb = new SqlPlayerDb(db);
            _cacheContainer = cacheContainer;
        }

        public Player Get(int id)
        {
            return _cacheContainer.GetAndStore(_playerDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<Player> Get(IList<int> ids)
        {
            return _cacheContainer.GetAndStore(_playerDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<Player> List(int bunchId)
        {
            var ids = _playerDb.Find(bunchId);
            return Get(ids);
        }

        public Player Get(int bunchId, int userId)
        {
            var ids = _playerDb.Find(bunchId, userId);
            if (!ids.Any())
                return null;
            return Get(ids).First();

        }

        public int Add(Player player)
        {
            return _playerDb.Add(player);
        }

        public bool JoinBunch(Player player, Bunch bunch, int userId)
        {
            return _playerDb.JoinHomegame(player, bunch, userId);
        }

        public void Delete(int playerId)
        {
            _playerDb.Delete(playerId);
            _cacheContainer.Remove<Player>(playerId);
        }
    }
}