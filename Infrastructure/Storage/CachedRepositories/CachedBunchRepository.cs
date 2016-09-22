using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Infrastructure.Storage.CachedRepositories
{
    public class CachedBunchRepository : IBunchRepository
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly ICacheContainer _cacheContainer;

        public CachedBunchRepository(IBunchRepository bunchRepository, ICacheContainer cacheContainer)
        {
            _bunchRepository = bunchRepository;
            _cacheContainer = cacheContainer;
        }

        public Bunch Get(int id)
        {
            return _cacheContainer.GetAndStore(_bunchRepository.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<Bunch> Get(IList<int> ids)
        {
            return _cacheContainer.GetAndStore(_bunchRepository.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
        }

        public IList<int> Search()
        {
            return _bunchRepository.Search();
        }

        public IList<int> Search(string slug)
        {
            return _bunchRepository.Search(slug);
        }

        public IList<int> Search(int userId)
        {
            return _bunchRepository.Search(userId);
        }

        public int Add(Bunch bunch)
        {
            return _bunchRepository.Add(bunch);
        }

        public void Update(Bunch bunch)
        {
            _bunchRepository.Update(bunch);
            _cacheContainer.Remove<Bunch>(bunch.Id);
        }
    }
}