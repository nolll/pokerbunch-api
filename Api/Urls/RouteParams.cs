namespace Api.Urls
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

        public static RouteReplace Slug(string slug) => new RouteReplace("slug", slug);
        public static RouteReplace Year(int year) => new RouteReplace("year?", year.ToString());
        public static RouteReplace PlayerId(string playerId) => new RouteReplace("playerId", playerId);
        public static RouteReplace CashgameId(string cashgameId) => new RouteReplace("cashgameId", cashgameId);
        public static RouteReplace Id(int id) => new RouteReplace("id", id.ToString());
        public static RouteReplace Id(string id) => new RouteReplace("id", id);
        public static RouteReplace UserName(string userName) => new RouteReplace("userName", userName);
        public static RouteReplace Code(string code) => new RouteReplace("code", code);
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