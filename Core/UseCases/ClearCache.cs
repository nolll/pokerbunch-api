using Core.Errors;
using Core.Services;

namespace Core.UseCases;

public class ClearCache(ICache cache) : UseCase<ClearCache.Request, ClearCache.Result>
{
    protected override Task<UseCaseResult<Result>> Work(Request request)
    {
        if (!request.AccessControl.CanClearCache)
            return Task.FromResult(Error(new AccessDeniedError()));

        cache.ClearAll();

        return Task.FromResult(Success(new Result()));
    }

    public class Request(IAccessControl accessControl)
    {
        public IAccessControl AccessControl { get; } = accessControl;
    }

    public class Result
    {
        public string Message => "The cache was cleared";
    }
}