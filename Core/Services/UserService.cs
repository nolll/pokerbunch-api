using System.Collections.Generic;
using Core.Entities;
using Core.Repositories;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Get(int id)
        {
            return _userRepository.Get(id);
        }

        public User GetByNameOrEmail(string nameOrEmail)
        {
            return _userRepository.Get(nameOrEmail);
        }

        public IList<User> List()
        {
            return _userRepository.List();
        }
        
        public void Save(User user)
        {
            _userRepository.Update(user);
        }

        public int Add(User user)
        {
            return _userRepository.Add(user);
        }
    }
}
