using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class ClearCache(ICache cache, IUserRepository userRepository) : UseCase<ClearCache.Request, ClearCache.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var user = await userRepository.GetByUserName(request.UserName);
        if (!AccessControl.CanClearCache(user))
            return Error(new AccessDeniedError());

        cache.ClearAll();

        return Success(new Result());
    }

    public class Request(string userName)
    {
        public string UserName { get; } = userName;
    }

    public class Result
    {
        public string Message => "The cache was cleared";
    }
}