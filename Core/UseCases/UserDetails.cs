using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class UserDetails(IUserRepository userRepository) : UseCase<UserDetails.Request, UserDetails.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var currentUser = await userRepository.GetByUserName(request.CurrentUserName);
        var displayUser = await userRepository.GetByUserName(request.UserName);

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
    
    public class Request(string currentUserName, string? userName = null)
    {
        public string CurrentUserName { get; } = currentUserName;
        public string UserName { get; } = userName ?? currentUserName;
    }

    public class Result(
        string userName,
        string displayName,
        string realName,
        string email,
        string avatarUrl,
        Role role,
        bool canViewAll)
    {
        public string UserName { get; } = userName;
        public string DisplayName { get; } = displayName;
        public string RealName { get; } = realName;
        public string Email { get; } = email;
        public string AvatarUrl { get; } = avatarUrl;
        public Role Role { get; } = role;
        public bool CanViewAll { get; } = canViewAll;
    }
}