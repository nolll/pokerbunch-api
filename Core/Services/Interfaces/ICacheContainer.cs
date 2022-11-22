using System;
using Core.Entities;

namespace Core.Services;

public interface ICacheContainer
{
    void Remove(string cacheKey);
    void Remove<T>(string id);
    void ClearAll();
    T GetAndStore<T>(Func<T> sourceExpression, TimeSpan cacheTime, string cacheKey) where T : class;
    Task<T> GetAndStoreAsync<T>(Func<Task<T>> sourceExpression, TimeSpan cacheTime, string cacheKey) where T : class;
    T GetAndStore<T>(Func<string, T> sourceExpression, string id, TimeSpan cacheTime) where T : class, IEntity;
    Task<T> GetAndStoreAsync<T>(Func<string, Task<T>> sourceExpression, string id, TimeSpan cacheTime) where T : class, IEntity;
    IList<T> GetAndStore<T>(Func<IList<string>, IList<T>> sourceExpression, IList<string> ids, TimeSpan cacheTime) where T : class, IEntity;
    Task<IList<T>> GetAndStoreAsync<T>(Func<IList<string>, Task<IList<T>>> sourceExpression, IList<string> ids, TimeSpan cacheTime) where T : class, IEntity;
}
