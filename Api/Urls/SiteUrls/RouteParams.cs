namespace PokerBunch.Common.Urls.SiteUrls
{
    public class RouteReplace
    {
        public string Pattern { get; }
        public string Value { get; }

        private RouteReplace(string pattern, string value)
        {
            Pattern = pattern;
            Value = value;
        }

        public static RouteReplace ActionId(string actionId) => new RouteReplace(nameof(actionId), actionId);
        public static RouteReplace AppId(string appId) => new RouteReplace(nameof(appId), appId);
        public static RouteReplace BunchId(string bunchId) => new RouteReplace(nameof(bunchId), bunchId);
        public static RouteReplace CashgameId(string cashgameId) => new RouteReplace(nameof(cashgameId), cashgameId);
        public static RouteReplace Code(string code) => new RouteReplace(nameof(code), code);
        public static RouteReplace EventId(string eventId) => new RouteReplace(nameof(eventId), eventId);
        public static RouteReplace LocationId(string locationId) => new RouteReplace(nameof(locationId), locationId);
        public static RouteReplace PlayerId(string playerId) => new RouteReplace(nameof(playerId), playerId);
        public static RouteReplace UserName(string userName) => new RouteReplace(nameof(userName), userName);
        public static RouteReplace Year(int year) => new RouteReplace(nameof(year), year.ToString());
    }

    public static class RouteParams
    {
        public static string Replace(string format, params RouteReplace[] routeReplaces)
        {
            var result = format;
            foreach (var rp in routeReplaces)
            {
                result = Replace(result, $"{{{rp.Pattern}}}", rp.Value);
            }
            return result;
        }

        private static string Replace(string format, string key, string value)
        {
            return format.Replace(key, value);
        }
    }
}