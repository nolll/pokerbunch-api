using Core.Entities;

namespace Core.Repositories;

public interface ILocationRepository
{
    Task<Location> Get(string id);
    Task<IList<Location>> List(IList<string> ids);
    Task<IList<Location>> List(string bunchId);
    Task<string> Add(Location location);
}