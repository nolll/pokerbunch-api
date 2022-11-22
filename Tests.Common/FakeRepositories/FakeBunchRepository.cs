using Core.Entities;
using Core.Repositories;

namespace Tests.Common.FakeRepositories;

public class FakeBunchRepository : IBunchRepository
{
    public Bunch Added { get; private set; }
    public Bunch Saved { get; private set; }
    private IList<Bunch> _list; 

    public FakeBunchRepository()
    {
        SetupDefaultList();
    }

    public Task<Bunch> Get(string id)
    {
        return Task.FromResult(_list.First(o => o.Id == id));
    }

    public Task<IList<Bunch>> List(IList<string> ids)
    {
        return Task.FromResult<IList<Bunch>>(_list.Where(o => ids.Contains(o.Id)).ToList());
    }

    public Task<Bunch> GetBySlug(string slug)
    {
        var bunch = _list.FirstOrDefault(o => o.Slug == slug);
        return Task.FromResult(bunch);
    }

    public Task<IList<Bunch>> GetByUserId(string userId)
    {
        return Task.FromResult(_list);
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
        Added = bunch;
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