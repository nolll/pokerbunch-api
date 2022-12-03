using Core.Entities;

namespace Core.Repositories;

public interface IUserRepository
{
    Task<User> GetById(string id);
    Task<User> GetByUserName(string userName);
    Task<User> GetByUserEmail(string email);
    Task<User> GetByUserNameOrEmail(string userNameOrEmail);
    Task<IList<User>> List();
    Task Update(User user);
    Task<string> Add(User user);
}