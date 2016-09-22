using System;
using System.Collections.Generic;
using Core.Entities;

namespace Core.Services
{
    public interface ICacheContainer
    {
        void Remove(string cacheKey);
        void Remove<T>(int id);
        int ClearAll();
        T GetAndStore<T>(Func<T> sourceExpression, TimeSpan cacheTime, string cacheKey) where T : class;
        T GetAndStore<T>(Func<int, T> sourceExpression, int id, TimeSpan cacheTime) where T : class, IEntity;
        IList<T> GetAndStore<T>(Func<IList<int>, IList<T>> sourceExpression, IList<int> ids, TimeSpan cacheTime) where T : class, IEntity;
    }
}