using System.Collections.Generic;
using Core.Entities;

namespace Core.Repositories
{
    public interface IUserRepository
    {
        User Get(int id);
        IList<User> Get(IList<int> ids);
        IList<int> Find();
        IList<int> Find(string nameOrEmail);
        void Update(User user);
        int Add(User user);
    }
}