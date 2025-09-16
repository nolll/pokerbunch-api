using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class UserDetails(IUserRepository userRepository) : UseCase<UserDetails.Request, UserDetails.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var displayUser = await userRepository.GetByUserName(request.UserName);
        var isViewingCurrentUser = displayUser.UserName == request.Auth.UserName;
        var userName = displayUser.UserName;
        var displayName = displayUser.DisplayName;
        var realName = displayUser.RealName;
        var email = displayUser.Email;
        var avatarUrl = GravatarService.GetAvatarUrl(displayUser.Email);
        var role = displayUser.GlobalRole;
        var canViewAll = request.Auth.CanViewFullUserData || isViewingCurrentUser;

        return Success(new Result(userName, displayName, realName, email, avatarUrl, role, canViewAll));
    }
    
    public class Request(IAuth auth, string? userName = null)
    {
        public IAuth Auth { get; } = auth;
        public string UserName { get; } = userName ?? auth.UserName;
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