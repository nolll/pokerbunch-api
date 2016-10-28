using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Exceptions;
using Core.Repositories;

namespace Tests.Common.FakeRepositories
{
    public class FakeBunchRepository : IBunchRepository
    {
        public Bunch Added { get; private set; }
        public Bunch Saved { get; private set; }
        private IList<Bunch> _list; 

        public FakeBunchRepository()
        {
            SetupDefaultList();
        }

        public Bunch Get(int id)
        {
            return _list.First(o => o.Id == id);
        }

        public IList<Bunch> List(IList<int> ids)
        {
            return _list.Where(o => ids.Contains(o.Id)).ToList();
        }

        public Bunch GetBySlug(string slug)
        {
            var bunch = _list.FirstOrDefault(o => o.Slug == slug);
            if(bunch == null)
                throw new BunchNotFoundException(slug);
            return bunch;
        }

        public IList<Bunch> GetByUserId(int userId)
        {
            return _list;
        }

        public IList<Bunch> List()
        {
            return _list;
        }

        public IList<Bunch> List(int userId)
        {
            return _list;
        }

        public int Add(Bunch bunch)
        {
            Added = bunch;
            return 1;
        }

        public void Update(Bunch bunch)
        {
            Saved = bunch;
        }

        public void SetupDefaultList()
        {
            _list = new List<Bunch>
            {
                TestData.BunchA,
                TestData.BunchB
            };
        }

        public void SetupOneBunchList()
        {
            _list = new List<Bunch>
            {
                TestData.BunchA
            };
        }

        public void ClearList()
        {
            _list.Clear();
        }
    }
}