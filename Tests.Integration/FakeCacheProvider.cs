using Core.Services;

namespace Tests.Integration;

public class FakeCacheProvider : ICacheProvider
{
    public object Get(string key)
    {
        return null;
    }

    public void Put(string key, object obj, TimeSpan time)
    {
    }

    public void Remove(string key)
    {
    }

    public void ClearAll()
    {
    }
}