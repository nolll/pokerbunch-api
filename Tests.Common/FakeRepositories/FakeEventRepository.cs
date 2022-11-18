using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Repositories;

namespace Tests.Common.FakeRepositories;

public class FakeEventRepository : IEventRepository
{
    public Event Added { get; private set; }
    public int AddedCashgameId { get; private set; }

    private readonly IList<Event> _list;

    public FakeEventRepository()
    {
        _list = CreateEventList();
    }

    public Task<Event> Get(int id)
    {
        return Task.FromResult(_list.FirstOrDefault(o => o.Id == id));
    }
        
    public Task<IList<Event>> Get(IList<int> ids)
    {
        return Task.FromResult<IList<Event>>(_list.Where(o => ids.Contains(o.Id)).ToList());
    }

    public Task<IList<Event>> List(int bunchId)
    {
        return Task.FromResult<IList<Event>>(_list.Where(o => o.BunchId == bunchId).ToList());
    }

    public Task<Event> GetByCashgame(int cashgameId)
    {
        return Task.FromResult(_list.First());
    }

    public Task<int> Add(Event e)
    {
        Added = e;
        return Task.FromResult(1);
    }

    public Task AddCashgame(int eventId, int cashgameId)
    {
        AddedCashgameId = cashgameId;
        return Task.CompletedTask;
    }

    private IList<Event> CreateEventList()
    {
        return new List<Event>
        {
            new(TestData.EventIdA, TestData.BunchA.Id, TestData.EventNameA, TestData.LocationIdA, new Date(TestData.StartTimeA), new Date(TestData.StartTimeA.AddDays(1))),
            new(TestData.EventIdB, TestData.BunchA.Id, TestData.EventNameB, TestData.LocationIdB, new Date(TestData.StartTimeB), new Date(TestData.StartTimeB.AddDays(1)))
        };
    }
}