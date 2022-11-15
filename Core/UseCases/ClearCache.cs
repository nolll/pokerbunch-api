using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class ClearCache : UseCase<ClearCache.Result, ClearCache.Request>
{
    private readonly ICacheContainer _cache;
    private readonly IUserRepository _userRepository;

    public ClearCache(ICacheContainer cache, IUserRepository userRepository)
    {
        _cache = cache;
        _userRepository = userRepository;
    }

    protected override UseCaseResult<Result> Work(Request request)
    {
        var user = _userRepository.Get(request.UserName);
        if (!AccessControl.CanClearCache(user))
            return new UseCaseResult<Result>(new AccessDeniedError());

        _cache.ClearAll();

        return new UseCaseResult<Result>(new Result());
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