using System;
using System.Text;

namespace Core.Services;

public static class RandomStringGenerator
{
    private static readonly Random Random = new();

    public static string GetString(int stringLength, string allowedCharacters)
    {
        if (string.IsNullOrEmpty(allowedCharacters))
            return "";
        var max = allowedCharacters.Length - 1;
        var str = new StringBuilder();
        for(var i = 0; i < stringLength; i++)
        {
            var randomPos = Random.Next(max);
            str.Append(allowedCharacters.Substring(randomPos, 1));
        }
        return str.ToString();
    }
}