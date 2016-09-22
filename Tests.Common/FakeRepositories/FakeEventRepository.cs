using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;

namespace Tests.Common.FakeRepositories
{
    public class FakeEventRepository : IEventRepository
    {
        public Event Added { get; private set; }
        public int AddedCashgameId { get; private set; }

        private readonly IList<Event> _list;

        public FakeEventRepository()
        {
            _list = CreateEventList();
        }

        public Event Get(int id)
        {
            return _list.FirstOrDefault(o => o.Id == id);
        }
        
        public IList<Event> Get(IList<int> ids)
        {
            return _list.Where(o => ids.Contains(o.Id)).ToList();
        }

        public IList<int> FindByBunchId(int bunchId)
        {
            return _list.Where(o => o.BunchId == bunchId).Select(o => o.Id).ToList();
        }

        public IList<int> FindByCashgameId(int cashgameId)
        {
            return new List<int>(_list.First().Id);
        }

        public int Add(Event e)
        {
            Added = e;
            return 1;
        }

        public void AddCashgame(int eventId, int cashgameId)
        {
            AddedCashgameId = cashgameId;
        }

        private IList<Event> CreateEventList()
        {
            return new List<Event>
            {
                new Event(TestData.EventIdA, TestData.BunchA.Id, TestData.EventNameA, TestData.LocationIdA, new Date(TestData.StartTimeA), new Date(TestData.StartTimeA.AddDays(1))),
                new Event(TestData.EventIdB, TestData.BunchA.Id, TestData.EventNameB, TestData.LocationIdB, new Date(TestData.StartTimeB), new Date(TestData.StartTimeB.AddDays(1)))
            };
        }
    }
}