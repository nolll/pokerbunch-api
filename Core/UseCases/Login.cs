using Core.Entities;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class Login
    {
        private readonly IUserRepository _userRepository;

        public Login(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Result Execute(Request request)
        {
            var user = GetLoggedInUser(request.LoginName, request.Password);

            if (user == null)
                throw new LoginException("There was something wrong with your username or password. Please try again.");
            return new Result(user.UserName);
        }

        private User GetLoggedInUser(string loginName, string password)
        {
            var user = _userRepository.Get(loginName);
            if (user == null)
                return null;

            var isValid = PasswordService.IsValid(password, user.Salt, user.EncryptedPassword);
            return isValid ? user : null;
        }

        public class Request 
        {
            public string LoginName { get; }
            public string Password { get; }

            public Request(string loginName, string password)
            {
                LoginName = loginName;
                Password = password;
            }
        }

        public class Result
        {
            public string UserName { get; }

            public Result(string userName)
            {
                UserName = userName;
            }
        }
    }
}