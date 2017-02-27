using Core.Services;

namespace Core.UseCases
{
    public class ClearCache
    {
        private readonly ICacheContainer _cache;

        public ClearCache(ICacheContainer cache)
        {
            _cache = cache;
        }

        public Result Execute()
        {
            var clearCount = _cache.ClearAll();

            return new Result(clearCount);
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
