using Core.Exceptions;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class ClearCache
{
    private readonly ICacheContainer _cache;
    private readonly IUserRepository _userRepository;

    public ClearCache(ICacheContainer cache, IUserRepository userRepository)
    {
        _cache = cache;
        _userRepository = userRepository;
    }

    public void Execute(Request request)
    {
        var user = _userRepository.Get(request.UserName);
        if (!AccessControl.CanClearCache(user))
            throw new AccessDeniedException();

        _cache.ClearAll();
    }

    public class Request
    {
        public string UserName { get; }

        public Request(string userName)
        {
            UserName = userName;
        }
    }
}