using System;
using System.Linq;
using Core.Entities;
using Core.Services;

namespace Core.Cache;

public class Cache : ICache
{
    private readonly ICacheProvider _cacheProvider;

    public Cache(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }

    public void Remove<T>(string id)
    {
        Remove(CacheKeyProvider.GetKey<T>(id));
    }

    public void ClearAll()
    {
        _cacheProvider.ClearAll();
    }

    public T GetAndStore<T>(Func<T> sourceExpression, TimeSpan cacheTime, string cacheKey) where T : class
    {
        var foundInCache = TryGet(cacheKey, out T? cachedObject);

        if (foundInCache)
            return cachedObject!;
        
        cachedObject = sourceExpression();
        Insert(cacheKey, cachedObject, cacheTime);
        return cachedObject;
    }

    public async Task<T> GetAndStoreAsync<T>(Func<Task<T>> sourceExpression, TimeSpan cacheTime, string cacheKey) where T : class
    {
        var foundInCache = TryGet(cacheKey, out T? cachedObject);

        if (foundInCache)
            return cachedObject!;
        
        cachedObject = await sourceExpression();
        Insert(cacheKey, cachedObject, cacheTime);
        return cachedObject;
    }

    public T GetAndStore<T>(Func<string, T> sourceExpression, string id, TimeSpan cacheTime) where T : class, IEntity
    {
        var cacheKey = CacheKeyProvider.GetKey<T>(id);
        var foundInCache = TryGet(cacheKey, out T? cachedObject);

        if (foundInCache)
            return cachedObject!;

        cachedObject = sourceExpression(id);
        Insert(cacheKey, cachedObject, cacheTime);
        return cachedObject;
    }

    public async Task<T> GetAndStoreAsync<T>(Func<string, Task<T>> sourceExpression, string id, TimeSpan cacheTime) where T : class, IEntity
    {
        var cacheKey = CacheKeyProvider.GetKey<T>(id);
        var foundInCache = TryGet(cacheKey, out T? cachedObject);

        if (foundInCache)
            return cachedObject!;

        cachedObject = await sourceExpression(id);
        Insert(cacheKey, cachedObject, cacheTime);
        return cachedObject;
    }

    public IList<T> GetAndStore<T>(Func<IList<string>, IList<T>> sourceExpression, IList<string> ids, TimeSpan cacheTime) where T : class, IEntity
    {
        var list = new List<T>();
        var notInCache = new List<string>();
        foreach (var id in ids)
        {
            var cacheKey = CacheKeyProvider.GetKey<T>(id);
            var foundInCache = TryGet(cacheKey, out T? cachedObject);
            if (foundInCache)
                list.Add(cachedObject!);
            else
                notInCache.Add(id);
        }

        if (notInCache.Any())
        {
            var sourceItems = sourceExpression(notInCache);
            foreach (var sourceItem in sourceItems)
            {
                if (sourceItem != null) //Om n�got id inte har h�mtats s� stoppar vi inte in det i v�rt resultat eller i cachen.
                {
                    var cacheKey = CacheKeyProvider.GetKey<T>(sourceItem.Id);
                    Insert(cacheKey, sourceItem, cacheTime);
                }
            }

            list = list.Concat(sourceItems.Where(o => o != null)).ToList();
            return OrderItemsByIdList(ids, list);
        }

        return list;
    }

    public async Task<IList<T>> GetAndStoreAsync<T>(Func<IList<string>, Task<IList<T>>> sourceExpression, IList<string> ids, TimeSpan cacheTime) where T : class, IEntity
    {
        var list = new List<T>();
        var notInCache = new List<string>();
        foreach (var id in ids)
        {
            var cacheKey = CacheKeyProvider.GetKey<T>(id);
            var foundInCache = TryGet(cacheKey, out T? cachedObject);
            if (foundInCache)
                list.Add(cachedObject!);
            else
                notInCache.Add(id);
        }

        if (notInCache.Any())
        {
            var sourceItems = await sourceExpression(notInCache);
            foreach (var sourceItem in sourceItems)
            {
                if (sourceItem != null)
                {
                    var cacheKey = CacheKeyProvider.GetKey<T>(sourceItem.Id);
                    Insert(cacheKey, sourceItem, cacheTime);
                }
            }

            list = list.Concat(sourceItems.Where(o => o != null)).ToList();
            return OrderItemsByIdList(ids, list);
        }
        return list;
    }

    private static IList<T> OrderItemsByIdList<T>(IEnumerable<string> ids, IEnumerable<T> list) where T : class, IEntity
    {
        var result = ids.Select(id => list.FirstOrDefault(i => i.Id == id)).ToList();
        return result.Where(r => r != null).ToList()!;
    }

    private bool TryGet<T>(string key, out T? value) where T : class
    {
        // Uncomment this row to temporarily disable cache in development
        //value = null; return false;

        var o = _cacheProvider.Get(key);

        if (o == null)
        {
            // A real null was found, this means that 'nothing is cached for this key
            value = default(T);
            return false;
        }

        value = (T)o;
        return true;
    }

    private void Insert(string cacheKey, object objectToBeCached, TimeSpan cacheTime)
    {
        _cacheProvider.Put(cacheKey, objectToBeCached, cacheTime);
    }

    public void Remove(string cacheKey)
    {
        _cacheProvider.Remove(cacheKey);
    }
}