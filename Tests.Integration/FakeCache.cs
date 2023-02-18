using Core.Entities;
using Core.Services;

namespace Tests.Integration;

public class FakeCache : ICache
{
    public void Remove(string cacheKey)
    {
    }

    public void Remove<T>(string id)
    {
    }

    public void ClearAll()
    {
    }

    public T GetAndStore<T>(Func<T> sourceExpression, TimeSpan cacheTime, string cacheKey) where T : class
    {
        return sourceExpression();
    }

    public Task<T> GetAndStoreAsync<T>(Func<Task<T>> sourceExpression, TimeSpan cacheTime, string cacheKey) where T : class
    {
        return sourceExpression();
    }

    public T GetAndStore<T>(Func<string, T> sourceExpression, string id, TimeSpan cacheTime) where T : class, IEntity
    {
        return sourceExpression(id);
    }

    public Task<T> GetAndStoreAsync<T>(Func<string, Task<T>> sourceExpression, string id, TimeSpan cacheTime) where T : class, IEntity
    {
        return sourceExpression(id);
    }

    public IList<T> GetAndStore<T>(Func<IList<string>, IList<T>> sourceExpression, IList<string> ids, TimeSpan cacheTime) where T : class, IEntity
    {
        return sourceExpression(ids);
    }

    public Task<IList<T>> GetAndStoreAsync<T>(Func<IList<string>, Task<IList<T>>> sourceExpression, IList<string> ids, TimeSpan cacheTime) where T : class, IEntity
    {
        return sourceExpression(ids);
    }
}