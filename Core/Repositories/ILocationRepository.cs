using System.Collections.Generic;
using Core.Entities;

namespace Core.Repositories;

public interface ILocationRepository
{
    Location Get(int id);
    IList<Location> List(IList<int> ids);
    IList<Location> List(int bunchId);
    int Add(Location location);
}