using Core.Services;

namespace Core.UseCases
{
    public class EditUserForm
    {
        private readonly UserService _userService;

        public EditUserForm(UserService userService)
        {
            _userService = userService;
        }

        public Result Execute(Request request)
        {
            var user = _userService.GetByNameOrEmail(request.UserName);
            var userName = user.UserName;
            var realName = user.RealName;
            var displayName = user.DisplayName;
            var email = user.Email;

            return new Result(userName, realName, displayName, email);
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
            public string UserName { get; private set; }
            public string RealName { get; private set; }
            public string DisplayName { get; private set; }
            public string Email { get; private set; }

            public Result(string userName, string realName, string displayName, string email)
            {
                UserName = userName;
                RealName = realName;
                DisplayName = displayName;
                Email = email;
            }
        }
    }
}