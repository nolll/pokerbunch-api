using System;

namespace Core.Services;

public static class DateTimeService
{
    public static DateTime UtcFromUnixTimeStamp(double unixTimeStamp) => 
        new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp);
}