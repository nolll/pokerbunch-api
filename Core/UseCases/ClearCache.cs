using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class ClearCache
    {
        private readonly ICacheContainer _cache;
        private readonly IUserRepository _userRepository;

        public ClearCache(ICacheContainer cache, IUserRepository userRepository)
        {
            _cache = cache;
            _userRepository = userRepository;
        }

        public Result Execute(Request request)
        {
            var user = _userRepository.Get(request.UserName);
            RequireRole.Admin(user);

            var clearCount = _cache.ClearAll();

            return new Result(clearCount);
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
            public int ClearCount { get; }

            public Result(int clearCount)
            {
                ClearCount = clearCount;
            }
        }
    }
}
