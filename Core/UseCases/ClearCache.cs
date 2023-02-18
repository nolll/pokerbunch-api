using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class ClearCache : UseCase<ClearCache.Request, ClearCache.Result>
{
    private readonly ICache _cache;
    private readonly IUserRepository _userRepository;

    public ClearCache(ICache cache, IUserRepository userRepository)
    {
        _cache = cache;
        _userRepository = userRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var user = await _userRepository.GetByUserName(request.UserName);
        if (!AccessControl.CanClearCache(user))
            return Error(new AccessDeniedError());

        _cache.ClearAll();

        return Success(new Result());
    }

    public class Request
    {
        public string UserName { get; }

        public Request(string userName)
        {
            UserName = userName;
        }
    }

    public class Result
    {
        public string Message { get; }

        public Result()
        {
            Message = "The cache was cleared";
        }
    }
}