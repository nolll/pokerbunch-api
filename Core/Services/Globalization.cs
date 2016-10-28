using System;
using System.Collections.Generic;
using System.Globalization;
using Core.Entities;

namespace Core.Services
{
    public static class Globalization
    {
        public static string FormatNumber(int number)
        {
            var culture = CultureInfo.CreateSpecificCulture("sv-SE");
            return number.ToString("N0", culture);
        }

        public static string FormatCurrency(Currency currency, int amount)
        {
            var numberFormatted = FormatNumber(amount);
            var amountFormatted = currency.Layout.Replace("{AMOUNT}", numberFormatted);
            return amountFormatted.Replace("{SYMBOL}", currency.Symbol);
        }

        public static string FormatWinrate(Currency currency, int winrate)
        {
            return FormatCurrency(currency, winrate) + "/h";
        }

        public static string FormatResult(Currency currency, int result)
        {
            var currencyValue = FormatCurrency(currency, result);
            if (result > 0)
                return "+" + currencyValue;
            return currencyValue;
        }

        public static string FormatDuration(int minutes)
        {
            var h = (int)Math.Floor((double)minutes / 60);
            var m = minutes % 60;
            if (h > 0 && m > 0)
                return h + "h " + m + "m";
            if (h > 0)
                return h + "h";
            return m + "m";
        }

        public static string FormatTimespan(TimeSpan timeSpan)
        {
            var minutes = (int)Math.Round(timeSpan.TotalMinutes);
            if (minutes == 0)
                return "now";
            if (minutes == 1)
                return "1 minute";
            return minutes + " minutes";
        }

        public static string FormatShortDate(DateTime dateTime, bool includeYear = false)
        {
            return dateTime.ToString(GetShortDateFormat(includeYear), CultureInfo.InvariantCulture);
        }

        public static string FormatShortDate(Date date, bool includeYear = false)
        {
            return FormatShortDate(date.UtcMidninght, includeYear);
        }

        public static string FormatShortDateTime(DateTime dateTime, bool includeYear = false)
        {
            return dateTime.ToString(GetShortDateTimeFormat(includeYear), CultureInfo.InvariantCulture);
        }

        public static string FormatTime(DateTime dateTime)
        {
            return dateTime.ToString("HH:mm", CultureInfo.InvariantCulture);
        }

        public static string FormatIsoDate(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public static string FormatIsoDateTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public static string FormatYear(DateTime dateTime)
        {
            return dateTime.ToString("yyyy", CultureInfo.InvariantCulture);
        }

        public static IEnumerable<TimeZoneInfo> Get()
        {
            return TimeZoneInfo.GetSystemTimeZones();
        }

        public static List<string> GetCurrencyLayouts()
        {
            return new List<string>
                {
                    "{SYMBOL} {AMOUNT}",
                    "{SYMBOL}{AMOUNT}",
                    "{AMOUNT}{SYMBOL}",
                    "{AMOUNT} {SYMBOL}"
                };
        }

        private static string GetShortDateFormat(bool includeYear = false)
        {
            return includeYear ? "MMM d yyyy" : "MMM d";
        }

        private static string GetShortDateTimeFormat(bool includeYear = false)
        {
            return includeYear ? "MMM d yyyy HH:mm" : "MMM d HH:mm";
        }
    }
}