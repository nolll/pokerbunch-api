using System.Collections.Generic;
using Core.Entities;

namespace Core.Services
{
    public interface IUserService
    {
        User GetById(int id);
        User GetByNameOrEmail(string nameOrEmail);
        IList<User> GetList();
        void Save(User user);
        int Add(User user);
    }
}