using System.Globalization;

namespace Api.Urls
{
    public static class RouteParams
    {
        private const string Slug = "{slug}";
        private const string Year = "{year}";
        private const string PlayerId = "{playerId}";
        private const string CashgameId = "{cashgameId}";
        private const string Id = "{id}";
        private const string UserName = "{userName}";

        public static string ReplaceSlug(string format, string slug)
        {
            return Replace(format, Slug, slug);
        }

        public static string ReplaceYear(string format, int year)
        {
            return Replace(format, Year, year);
        }

        public static string ReplacePlayerId(string format, int playerId)
        {
            return Replace(format, PlayerId, playerId);
        }

        public static string ReplaceCashgameId(string format, int cashgameId)
        {
            return Replace(format, CashgameId, cashgameId);
        }

        public static string ReplaceId(string format, int id)
        {
            return Replace(format, Id, id);
        }

        public static string ReplaceId(string format, string id)
        {
            return Replace(format, Id, id);
        }

        public static string ReplaceUserName(string format, string userName)
        {
            return Replace(format, UserName, userName);
        }

        private static string Replace(string format, string key, string value)
        {
            return format.Replace(key, value);
        }

        private static string Replace(string format, string key, int value)
        {
            return format.Replace(key, value.ToString(CultureInfo.InvariantCulture));
        }
    }
}