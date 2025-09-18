using Core.Entities;
using Core.Repositories;

namespace Tests.Common.FakeRepositories;

public class FakeEventRepository : IEventRepository
{
    public string? AddedCashgameId { get; private set; }

    private readonly IList<Event> _list;

    public FakeEventRepository()
    {
        _list = CreateEventList();
    }

    public Task<Event> Get(string id)
    {
        return Task.FromResult(_list.FirstOrDefault(o => o.Id == id))!;
    }
        
    public Task<IList<Event>> Get(IList<string> ids)
    {
        return Task.FromResult<IList<Event>>(_list.Where(o => ids.Contains(o.Id)).ToList());
    }

    public Task<IList<Event>> List(string bunchId)
    {
        return Task.FromResult<IList<Event>>(_list.Where(o => o.BunchId == bunchId).ToList());
    }

    public Task<Event?> GetByCashgame(string cashgameId)
    {
        return Task.FromResult(_list.FirstOrDefault());
    }

    public Task<string> Add(Event e)
    {
        return Task.FromResult("1");
    }

    public Task AddCashgame(string eventId, string cashgameId)
    {
        AddedCashgameId = cashgameId;
        return Task.CompletedTask;
    }

    public Task RemoveCashgame(string eventId, string cashgameId)
    {
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