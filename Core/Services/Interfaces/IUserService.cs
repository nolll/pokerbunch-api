using System.Collections.Generic;
using Core.Entities;

namespace Core.Services
{
    public interface IUserService
    {
        User Get(int id);
        User GetByNameOrEmail(string nameOrEmail);
        IList<User> List();
        void Save(User user);
        int Add(User user);
    }
}