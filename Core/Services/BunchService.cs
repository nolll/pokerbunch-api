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
            return _bunchRepository.GetBySlug(slug);
        }

        public IList<Bunch> List(int userId)
        {
            return _bunchRepository.List(userId);
        }

        public IList<Bunch> List()
        {
            return _bunchRepository.List();
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