using System;
using System.Runtime.Caching;
using Core.Services;

namespace Infrastructure.Cache;

public class MemoryCacheProvider : ICacheProvider
{
    private static MemoryCache _cache = new("memcache");

    public object? Get(string key) => _cache.Get(key);
    public void Put(string key, object obj, TimeSpan time) => _cache.Set(key, obj, new SlidingExpirationPolicy(time));
    public void Remove(string key) => _cache.Remove(key);

    public void ClearAll()
    {
        _cache.Dispose();
        _cache = new MemoryCache("memcache");
    }
}