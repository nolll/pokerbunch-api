using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class RequireAppsettingsAccess(IUserRepository userRepository)
    : UseCase<RequireAppsettingsAccess.Request, RequireAppsettingsAccess.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var user = await userRepository.GetByUserName(request.UserName);
        return !AccessControl.CanSeeAppSettings(user) 
            ? Error(new AccessDeniedError()) 
            : Success(new Result());
    }
    
    public class Request(string userName)
    {
        public string UserName { get; } = userName;
    }

    public class Result
    {
    }
}