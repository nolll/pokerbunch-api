using Core.Entities;

namespace Core.Repositories;

public interface IUserRepository
{
    Task<User> GetById(string id);
    Task<User> GetByUserNameOrEmail(string userNameOrEmail);
    Task<IList<User>> List();
    Task Update(User user);
    Task<string> Add(User user);
}