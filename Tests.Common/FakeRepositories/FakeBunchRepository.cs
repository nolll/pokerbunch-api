using Core.Entities;
using Core.Repositories;

namespace Tests.Common.FakeRepositories;

public class FakeBunchRepository : IBunchRepository
{
    public Bunch? Saved { get; private set; }
    private IList<Bunch> _list = new List<Bunch>(); 

    public FakeBunchRepository()
    {
        SetupDefaultList();
    }

    public Task<Bunch> Get(string id)
    {
        return Task.FromResult(_list.First(o => o.Id == id));
    }

    public Task<Bunch> GetBySlug(string slug)
    {
        var bunch = _list.FirstOrDefault(o => o.Slug == slug)!;
        return Task.FromResult(bunch);
    }

    public Task<Bunch?> GetBySlugOrNull(string slug)
    {
        var bunch = _list.FirstOrDefault(o => o.Slug == slug)!;
        return Task.FromResult<Bunch?>(bunch);
    }

    public Task<IList<Bunch>> List()
    {
        return Task.FromResult(_list);
    }

    public Task<IList<Bunch>> List(string userId)
    {
        return Task.FromResult(_list);
    }

    public Task<string> Add(Bunch bunch)
    {
        return Task.FromResult("1");
    }

    public async Task Update(Bunch bunch)
    {
        Saved = await Task.FromResult(bunch);
    }

    public void SetupDefaultList()
    {
        _list = new List<Bunch>
        {
            TestData.BunchA,
            TestData.BunchB
        };
    }
}