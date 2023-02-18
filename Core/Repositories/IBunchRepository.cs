using Core.Entities;

namespace Core.Repositories;

public interface IBunchRepository
{
    Task<Bunch> Get(string id);
    Task<Bunch> GetBySlug(string slug);
    Task<Bunch?> GetBySlugOrNull(string slug);
    Task<IList<Bunch>> List();
    Task<IList<Bunch>> List(string userId);
    Task<string> Add(Bunch bunch);
    Task Update(Bunch bunch);
}