using Core.Entities;

namespace Core.Repositories;

public interface IUserRepository
{
    Task<User> Get(int id);
    Task<User> Get(string nameOrEmail);
    Task<IList<User>> List();
    Task Update(User user);
    Task<int> Add(User user);
}