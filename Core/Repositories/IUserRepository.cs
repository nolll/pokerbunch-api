using System.Collections.Generic;
using Core.Entities;

namespace Core.Repositories;

public interface IUserRepository
{
    User Get(int id);
    User Get(string nameOrEmail);
    IList<User> List();
    void Update(User user);
    int Add(User user);
}