using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.Repositories;

namespace Infrastructure.Sql.CachedRepositories
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

        public IList<int> Find(int bunchId)
        {
            return _playerDb.Find(bunchId);
        }

        public IList<int> Find(int bunchId, string name)
        {
            return _playerDb.Find(bunchId, name);
        }

        public IList<int> Find(int bunchId, int userId)
        {
            return _playerDb.Find(bunchId, userId);
        }

        public int Add(Player player)
        {
            return _playerDb.Add(player);
        }

        public bool JoinHomegame(Player player, Bunch bunch, int userId)
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