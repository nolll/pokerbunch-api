using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class RequireAppsettingsAccess()
    : UseCase<RequireAppsettingsAccess.Request, RequireAppsettingsAccess.Result>
{
    protected override Task<UseCaseResult<Result>> Work(Request request)
    {
        return Task.FromResult(!request.Auth.CanSeeAppSettings 
            ? Error(new AccessDeniedError()) 
            : Success(new Result()));
    }
    
    public class Request(IAuth auth)
    {
        public IAuth Auth { get; } = auth;
    }

    public class Result
    {
    }
}