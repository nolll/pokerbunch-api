using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Services;

namespace Web.Common.Cache
{
    public class CacheContainer : ICacheContainer
    {
        private readonly ICacheProvider _cacheProvider;

        public CacheContainer(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public void Remove<T>(int id)
        {
            Remove(CacheKeyProvider.GetKey<T>(id));
        }

        public int ClearAll()
        {
            return _cacheProvider.ClearAll();
        }

        public T GetAndStore<T>(Func<T> sourceExpression, TimeSpan cacheTime, string cacheKey) where T : class
        {
            T cachedObject;
            var foundInCache = TryGet(cacheKey, out cachedObject);

            if (foundInCache)
            {
                return cachedObject;
            }
            cachedObject = sourceExpression();
            if (cachedObject != null)
            {
                Insert(cacheKey, cachedObject, cacheTime);
            }
            return cachedObject;
        }

        public T GetAndStore<T>(Func<int, T> sourceExpression, int id, TimeSpan cacheTime) where T : class, IEntity
        {
            T cachedObject;
            var cacheKey = CacheKeyProvider.GetKey<T>(id);
            var foundInCache = TryGet(cacheKey, out cachedObject);

            if (foundInCache)
            {
                return cachedObject;
            }
            cachedObject = sourceExpression(id);
            if (cachedObject != null)
            {
                Insert(cacheKey, cachedObject, cacheTime);
            }
            return cachedObject;
        }

        public IList<T> GetAndStore<T>(Func<IList<int>, IList<T>> sourceExpression, IList<int> ids, TimeSpan cacheTime) where T : class, IEntity
        {
            var list = new List<T>();
            var notInCache = new List<int>();
            foreach (var id in ids)
            {
                T cachedObject;
                var cacheKey = CacheKeyProvider.GetKey<T>(id);
                var foundInCache = TryGet(cacheKey, out cachedObject);
                if (foundInCache)
                    list.Add(cachedObject);
                else
                {
                    notInCache.Add(id);
                }
            }
            if (notInCache.Any())
            {
                var sourceItems = sourceExpression(notInCache);
                foreach (var sourceItem in sourceItems)
                {
                    if (sourceItem != null) //Om något id inte har hämtats så stoppar vi inte in det i vårt resultat eller i cachen.
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

        private static IList<T> OrderItemsByIdList<T>(IEnumerable<int> ids, IEnumerable<T> list) where T : class, IEntity
        {
            var result = ids.Select(id => list.FirstOrDefault(i => i.Id == id)).ToList();
            return result.Where(r => r != null).ToList();
        }

        private bool TryGet<T>(string key, out T value) where T : class
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
}