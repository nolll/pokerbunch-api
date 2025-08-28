using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class RequireAppsettingsAccess()
    : UseCase<RequireAppsettingsAccess.Request, RequireAppsettingsAccess.Result>
{
    protected override Task<UseCaseResult<Result>> Work(Request request)
    {
        return Task.FromResult(!request.AccessControl.CanSeeAppSettings 
            ? Error(new AccessDeniedError()) 
            : Success(new Result()));
    }
    
    public class Request(AccessControl accessControl)
    {
        public AccessControl AccessControl { get; } = accessControl;
    }

    public class Result
    {
    }
}