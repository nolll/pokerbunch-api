using Core.Entities;

namespace Core.Repositories;

public interface IBunchRepository
{
    Task<Bunch> Get(int id);
    Task<Bunch> GetBySlug(string slug);
    Task<IList<Bunch>> List();
    Task<IList<Bunch>> List(int userId);
    Task<int> Add(Bunch bunch);
    Task Update(Bunch bunch);
}