using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Repositories;

public interface IBunchRepository
{
    Bunch Get(int id);
    Bunch GetBySlug(string slug);
    IList<Bunch> List();
    IList<Bunch> List(int userId);
    Task<int> Add(Bunch bunch);
    Task Update(Bunch bunch);
}