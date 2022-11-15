using System;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public abstract class UseCase<TResult, TRequest>
{
    protected abstract UseCaseResult<TResult> Work(TRequest request);

    public UseCaseResult<TResult> Execute(TRequest request)
    {
        try
        {
            return Work(request);
        }
        catch (Exception e)
        {
            return new UseCaseResult<TResult>(new UseCaseError(e));
        }
    }
}

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

        return new UseCaseResult<Result>(new Result("The cache was cleared"));
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

        public Result(string message)
        {
            Message = message;
        }
    }
}