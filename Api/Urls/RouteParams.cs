namespace Api.Urls
{
    public class RouteParam
    {
        public string Pattern { get; }
        public string Value { get; }

        private RouteParam(string pattern, string value)
        {
            Pattern = pattern;
            Value = value;
        }

        public static RouteParam Slug(string slug) => new RouteParam("slug", slug);
        public static RouteParam Year(int year) => new RouteParam("year?", year.ToString());
        public static RouteParam PlayerId(string playerId) => new RouteParam("playerId", playerId);
        public static RouteParam CashgameId(string cashgameId) => new RouteParam("cashgameId", cashgameId);
        public static RouteParam Id(int id) => new RouteParam("id", id.ToString());
        public static RouteParam Id(string id) => new RouteParam("id", id);
        public static RouteParam UserName(string userName) => new RouteParam("userName", userName);
        public static RouteParam Code(string code) => new RouteParam("code", code);
    }

    public static class RouteParams
    {
        public static string Replace(string format, params RouteParam[] routeParams)
        {
            var result = format;
            foreach (var rp in routeParams)
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