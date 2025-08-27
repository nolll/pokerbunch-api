using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class ClearCache(ICache cache) : UseCase<ClearCache.Request, ClearCache.Result>
{
    protected override Task<UseCaseResult<Result>> Work(Request request)
    {
        if (!AccessControl.CanClearCache(request.CurrentUser))
            return Task.FromResult(Error(new AccessDeniedError()));

        cache.ClearAll();

        return Task.FromResult(Success(new Result()));
    }

    public class Request(CurrentUser currentUser)
    {
        public CurrentUser CurrentUser { get; } = currentUser;
    }

    public class Result
    {
        public string Message => "The cache was cleared";
    }
}