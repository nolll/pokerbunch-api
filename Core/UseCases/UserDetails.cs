using Core.Services;

namespace Core.UseCases
{
    public class UserDetails
    {
        private readonly IUserService _userService;

        public UserDetails(IUserService userService)
        {
            _userService = userService;
        }

        public Result Execute(Request request)
        {
            var currentUser = _userService.GetByNameOrEmail(request.CurrentUserName);
            var displayUser = _userService.GetByNameOrEmail(request.UserName);

            var isViewingCurrentUser = displayUser.UserName == currentUser.UserName;

            var userName = displayUser.UserName;
            var displayName = displayUser.DisplayName;
            var realName = displayUser.RealName;
            var email = displayUser.Email;
            var avatarUrl = GravatarService.GetAvatarUrl(displayUser.Email);
            var canEdit = currentUser.IsAdmin || isViewingCurrentUser;
            var canChangePassword = isViewingCurrentUser;

            return new Result(userName, displayName, realName, email, avatarUrl, canEdit, canChangePassword);
        }

        public class Request
        {
            public string CurrentUserName { get; }
            public string UserName { get; }

            public Request(string currentUserName, string userName)
            {
                CurrentUserName = currentUserName;
                UserName = userName;
            }
        }

        public class Result
        {
            public string UserName { get; private set; }
            public string DisplayName { get; private set; }
            public string RealName { get; private set; }
            public string Email { get; private set; }
            public string AvatarUrl { get; private set; }
            public bool CanEdit { get; private set; }
            public bool CanChangePassword { get; private set; }

            public Result(string userName, string displayName, string realName, string email, string avatarUrl, bool canEdit, bool canChangePassword)
            {
                UserName = userName;
                DisplayName = displayName;
                RealName = realName;
                Email = email;
                AvatarUrl = avatarUrl;
                CanEdit = canEdit;
                CanChangePassword = canChangePassword;
            }
        }
    }
}