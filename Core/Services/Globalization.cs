using System;
using System.Globalization;

namespace Core.Services;

public static class Globalization
{
    public static string FormatIsoDate(DateTime dateTime) => 
        dateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
}