using System;
using System.Runtime.Caching;

namespace Infrastructure.Cache;

public class SlidingExpirationPolicy : CacheItemPolicy
{
    public SlidingExpirationPolicy(TimeSpan timeSpan)
    {
        SlidingExpiration = timeSpan;
    }
}