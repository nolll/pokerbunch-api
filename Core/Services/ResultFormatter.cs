using System.Globalization;
using Core.Entities;

namespace Core.Services;

public static class ResultFormatter
{
    public static string FormatWinnings(int winnings)
    {
        if(winnings > 0)
            return "+"  + winnings;
        return winnings.ToString(CultureInfo.InvariantCulture);
    }

    public static string FormatWinnings(Money money)
    {
        var str = money.ToString();
        if (money.Amount > 0)
            return "+" + str;
        return str;
    }

    public static string FormatWinRate(Money winRate)
    {
        return FormatWinnings(winRate) + "/h";
    }
}