using System.Collections.Generic;
using Core.Entities;

namespace Core.Repositories
{
    public interface ILocationRepository
    {
        Location Get(int id);
        IList<Location> Get(IList<int> ids);
        IList<int> Find(int bunchId);
        IList<int> Find(int bunchId, string name);
        int Add(Location location);
    }
}