using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class RequireAppsettingsAccess()
    : UseCase<RequireAppsettingsAccess.Request, RequireAppsettingsAccess.Result>
{
    protected override Task<UseCaseResult<Result>> Work(Request request)
    {
        return Task.FromResult(!request.Principal.CanSeeAppSettings 
            ? Error(new AccessDeniedError()) 
            : Success(new Result()));
    }
    
    public class Request(IPrincipal principal)
    {
        public IPrincipal Principal { get; } = principal;
    }

    public class Result
    {
    }
}