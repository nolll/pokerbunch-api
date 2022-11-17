using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Repositories;

public interface ILocationRepository
{
    Task<Location> Get(int id);
    Task<IList<Location>> List(IList<int> ids);
    Task<IList<Location>> List(int bunchId);
    Task<int> Add(Location location);
}