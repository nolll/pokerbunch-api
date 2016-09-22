using Core.Entities;
using Core.Exceptions;
using Core.Services;

namespace Core.UseCases
{
    public class Login
    {
        private readonly UserService _userService;

        public Login(UserService userService)
        {
            _userService = userService;
        }

        public Result Execute(Request request)
        {
            var user = GetLoggedInUser(request.LoginName, request.Password);

            if (user == null)
                throw new LoginException();
            return new Result(user.UserName);
        }

        private User GetLoggedInUser(string loginName, string password)
        {
            var user = _userService.GetByNameOrEmail(loginName);
            if (user == null)
                return null;
            var encryptedPassword = EncryptionService.Encrypt(password, user.Salt);
            return encryptedPassword == user.EncryptedPassword ? user : null;
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
            public string UserName { get; private set; }

            public Result(string userName)
            {
                UserName = userName;
            }
        }
    }
}