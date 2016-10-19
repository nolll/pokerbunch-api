using System;
using System.Text;
using Core.Entities;

namespace Api.Cache
{
    public static class CacheKeyProvider
    {
        public static string GetKey<T>(int id)
        {
            return ConstructCacheKey(typeof(T).ToString(), id); ;
        }

        public static string AppKey(int id)
        {
            return ConstructCacheKey("App", id);
        }

        public static string UserKey(string nameOrEmail)
        {
            return ConstructCacheKey("UserByNameOrEmail", nameOrEmail);
        }

        public static string UserIdByNameOrEmailKey(string nameOrEmail)
        {
            return ConstructCacheKey("UserId", "nameoremail", nameOrEmail);
        }

        public static string UserIdsKey()
        {
            return ConstructCacheKey("UserIds", "all");
        }

        public static string BunchIdBySlugKey(string slug)
        {
            return ConstructCacheKey("HomegameId", "slug", slug);
        }

        public static string BunchIdsKey()
        {
            return ConstructCacheKey("HomegameIds", "all");
        }

        public static string PlayerIdsKey(int bunchId)
        {
            return ConstructCacheKey("PlayerIds", bunchId);
        }

        public static string PlayerIdByNameKey(int bunchId, string name)
        {
            return ConstructCacheKey("PlayerId", "name", bunchId, name);
        }

        public static string PlayerIdByUserIdKey(int bunchId, int userId)
        {
            return ConstructCacheKey("PlayerId", "user", bunchId, userId);
        }

        public static string CashgameIdByDateStringKey(int bunchId, string dateString)
        {
            return ConstructCacheKey("CashgameId", bunchId, dateString);
        }

        public static string CashgameIdByRunningKey(int bunchId)
        {
            return ConstructCacheKey("CashgameId", bunchId, "running");
        }

        public static string CashgameIdsKey(int bunchId, GameStatus? status = null, int? year = null)
        {
            return ConstructCacheKey("CashgameIds", bunchId, status, year);
        }

        public static string EventCashgameIdsKey(int eventId)
        {
            return ConstructCacheKey("EventCashgameIds", eventId);
        }

        public static string CashgameYearsKey(int bunchId)
        {
            return ConstructCacheKey("CashgameYears", bunchId);
        }
        
        public static string EventIdsKey(int bunchId)
        {
            return ConstructCacheKey("EventIds", bunchId);
        }

        public static string ConstructCacheKey(Type t, int id)
        {
            return ConstructCacheKey(t.ToString(), id);
        }

        public static string ConstructCacheKey(string typeName, params object[] procedureParameters)
        {
            // construct a cachekey in the format "typeName:parameter1value:parameter2value:"
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(typeName);
            stringBuilder.Append(":");

            foreach (var parameter in procedureParameters)
            {
                stringBuilder.Append(parameter);
                stringBuilder.Append(":");
            }

            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            return stringBuilder.ToString();
        }
    }
}