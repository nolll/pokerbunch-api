using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Infrastructure.Sql.CachedRepositories
{
    public class CachedUserRepository : IUserRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly ICacheContainer _cacheContainer;

        public CachedUserRepository(IUserRepository userRepository, ICacheContainer cacheContainer)
        {
            _userRepository = userRepository;
            _cacheContainer = cacheContainer;
        }

        public User Get(int id)
        {
            return _cacheContainer.GetAndStore(_userRepository.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<User> Get(IList<int> ids)
        {
            return _cacheContainer.GetAndStore(_userRepository.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
        }
        
        public IList<int> Find()
        {
            return _userRepository.Find();
        }

        public IList<int> Find(string nameOrEmail)
        {
            return _userRepository.Find(nameOrEmail);
        }

        public void Update(User user)
        {
            _userRepository.Update(user);
            _cacheContainer.Remove<User>(user.Id);
        }

        public int Add(User user)
        {
            return _userRepository.Add(user);
        }
    }
}