using Core.Exceptions;
using Core.Repositories;

namespace Core.UseCases
{
    public class CoreContext
    {
        private readonly IUserRepository _userRepository;

        public CoreContext(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Result Execute(Request request)
        {
            var isAuthenticated = !string.IsNullOrEmpty(request.UserName);
            var userName = isAuthenticated ? request.UserName : string.Empty;
            var user = isAuthenticated ? _userRepository.Get(userName) : null;
            if (isAuthenticated && user == null) // Broken auth cookie
                throw new NotLoggedInException();
            var userId = isAuthenticated ? user.Id : 0;
            var userDisplayName = isAuthenticated ? user.DisplayName : string.Empty;
            var isAdmin = isAuthenticated && user.IsAdmin;

            return new Result(
                isAuthenticated,
                isAdmin,
                userId,
                userName,
                userDisplayName);
        }

        public class Request
        {
            public string UserName { get; }

            public Request(string userName)
            {
                UserName = userName;
            }
        }

        public class Result
        {
            public bool IsLoggedIn { get; }
            public bool IsAdmin { get; }
            public int UserId { get; }
            public string UserDisplayName { get; }
            public string UserName { get; }

            public Result(
                bool isLoggedIn,
                bool isAdmin,
                int userId,
                string userName,
                string userDisplayName)
            {
                IsLoggedIn = isLoggedIn;
                IsAdmin = isAdmin;
                UserId = userId;
                UserDisplayName = userDisplayName;
                UserName = userName;
            }
        }
    }
}