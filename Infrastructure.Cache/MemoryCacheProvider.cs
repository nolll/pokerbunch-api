using System;
using System.Runtime.Caching;
using Core.Services;

namespace Infrastructure.Cache
{
    public class MemoryCacheProvider : ICacheProvider
    {
        private static MemoryCache Cache = new MemoryCache("memcache");

        public MemoryCacheProvider()
        {
        }

        public object Get(string key)
        {
            return Cache.Get(key);
        }

        public void Put(string key, object obj, TimeSpan time)
        {
            Cache.Set(key, obj, new SlidingExpirationPolicy(time));
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        public void ClearAll()
        {
            Cache.Dispose();
            Cache = new MemoryCache("memcache");
        }
    }
}
