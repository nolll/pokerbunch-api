using Plumbing;

namespace Api.Cache
{
    public class Bootstrapper
    {
        public UseCaseContainer UseCases { get; private set; }

        public Bootstrapper(string connectionString)
        {
            var cacheContainer = new CacheContainer(new AspNetCacheProvider());
            var deps = new Dependencies(cacheContainer, connectionString);
            UseCases = new UseCaseContainer(deps);
        }
    }
}