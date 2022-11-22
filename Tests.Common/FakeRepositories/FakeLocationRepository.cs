using Core.Entities;
using Core.Repositories;

namespace Tests.Common.FakeRepositories;

public class FakeLocationRepository : ILocationRepository
{
    private readonly IList<Location> _list;
    public Location Added { get; private set; }
        
    public FakeLocationRepository()
    {
        _list = CreateLocationList();
    }

    public Task<Location> Get(string id)
    {
        return Task.FromResult(_list.FirstOrDefault(o => o.Id == id));
    }

    public Task<IList<Location>> List(IList<string> ids)
    {
        return Task.FromResult<IList<Location>>(_list.Where(o => ids.Contains(o.Id)).ToList());
    }

    public Task<IList<Location>> List(string bunchId)
    {
        return Task.FromResult<IList<Location>>(_list.Where(o => o.BunchId == bunchId).ToList());
    }
        
    public Task<string> Add(Location location)
    {
        Added = location;
        const string id = "1000";
        _list.Add(new Location(id, location.Name, location.BunchId));
        return Task.FromResult(id);
    }

    private IList<Location> CreateLocationList()
    {
        return new List<Location>
        {
            new(TestData.LocationIdA, TestData.LocationNameA, TestData.BunchA.Id),
            new(TestData.LocationIdB, TestData.LocationNameB, TestData.BunchA.Id),
            new(TestData.LocationIdC, TestData.LocationNameC, TestData.BunchA.Id),
            new(TestData.ChangedLocationId, TestData.ChangedLocationName, TestData.BunchA.Id)
        };
    }
}