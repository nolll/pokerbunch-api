using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Exceptions;
using Core.Repositories;

namespace Core.Services
{
    public class BunchService : IBunchService
    {
        private readonly IBunchRepository _bunchRepository;

        public BunchService(IBunchRepository bunchRepository)
        {
            _bunchRepository = bunchRepository;
        }

        public Bunch Get(int id)
        {
            return _bunchRepository.Get(id);
        }

        public Bunch GetBySlug(string slug)
        {
            var ids = _bunchRepository.Search(slug);
            if(ids.Any())
                return _bunchRepository.Get(ids.First());
            throw new BunchNotFoundException(slug);
        }

        public IList<Bunch> GetByUserId(int userId)
        {
            var ids = _bunchRepository.Search(userId);
            return _bunchRepository.Get(ids);
        }

        public IList<Bunch> GetList()
        {
            var ids = _bunchRepository.Search();
            return _bunchRepository.Get(ids);
        }

        public int Add(Bunch bunch)
        {
            return _bunchRepository.Add(bunch);
        }

        public void Save(Bunch bunch)
        {
            _bunchRepository.Update(bunch);
        }
    }
}