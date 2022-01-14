using System.Text;

namespace Core.Cache;

public static class CacheKeyProvider
{
    public static string GetKey<T>(int id)
    {
        return ConstructCacheKey(typeof(T).ToString(), id); ;
    }

    private static string ConstructCacheKey(string typeName, params object[] procedureParameters)
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