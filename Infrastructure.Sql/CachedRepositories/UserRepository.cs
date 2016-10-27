using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.Repositories;

namespace Infrastructure.Sql.CachedRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlUserDb _userDb;
        private readonly ICacheContainer _cacheContainer;

        public UserRepository(SqlServerStorageProvider db, ICacheContainer cacheContainer)
        {
            _userDb = new SqlUserDb(db);
            _cacheContainer = cacheContainer;
        }

        public User Get(int id)
        {
            return _cacheContainer.GetAndStore(_userDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<User> List()
        {
            var ids = _userDb.Find();
            return _userDb.Get(ids);
        }

        public User Get(string nameOrEmail)
        {
            var id = _userDb.Find(nameOrEmail);
            if (id == 0)
                return null;
            return Get(id);
        }

        public void Update(User user)
        {
            _userDb.Update(user);
            _cacheContainer.Remove<User>(user.Id);
        }

        public int Add(User user)
        {
            return _userDb.Add(user);
        }
    }
}