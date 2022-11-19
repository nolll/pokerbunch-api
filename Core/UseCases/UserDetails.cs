using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class UserDetails : UseCase<UserDetails.Request, UserDetails.Result>
{
    private readonly IUserRepository _userRepository;

    public UserDetails(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var currentUser = await _userRepository.Get(request.CurrentUserName);
        var displayUser = await _userRepository.Get(request.UserName);

        if (displayUser == null)
            return Error(new UserNotFoundError(request.UserName));
        
        var isViewingCurrentUser = displayUser.UserName == currentUser.UserName;
        var userName = displayUser.UserName;
        var displayName = displayUser.DisplayName;
        var realName = displayUser.RealName;
        var email = displayUser.Email;
        var avatarUrl = GravatarService.GetAvatarUrl(displayUser.Email);
        var role = displayUser.GlobalRole;
        var canViewAll = currentUser.IsAdmin || isViewingCurrentUser;

        return Success(new Result(userName, displayName, realName, email, avatarUrl, role, canViewAll));
    }
    
    public class Request
    {
        public string CurrentUserName { get; }
        public string UserName { get; }

        public Request(string currentUserName, string userName = null)
        {
            CurrentUserName = currentUserName;
            UserName = userName ?? currentUserName;
        }
    }

    public class Result
    {
        public string UserName { get; }
        public string DisplayName { get; }
        public string RealName { get; }
        public string Email { get; }
        public string AvatarUrl { get; }
        public Role Role { get; }
        public bool CanViewAll { get; }

        public Result(string userName, string displayName, string realName, string email, string avatarUrl, Role role, bool canViewAll)
        {
            UserName = userName;
            DisplayName = displayName;
            RealName = realName;
            Email = email;
            AvatarUrl = avatarUrl;
            Role = role;
            CanViewAll = canViewAll;
        }
    }
}