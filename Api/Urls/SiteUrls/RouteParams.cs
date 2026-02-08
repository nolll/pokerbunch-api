using System.Linq;

namespace Api.Urls.SiteUrls;

public class RouteReplace
{
    public string Pattern { get; }
    public string Value { get; }

    private RouteReplace(string pattern, string value)
    {
        Pattern = pattern;
        Value = value;
    }

    public static RouteReplace ActionId(string actionId) => new(nameof(actionId), actionId);
    public static RouteReplace BunchId(string bunchId) => new(nameof(bunchId), bunchId);
    public static RouteReplace JoinRequestId(string joinRequestId) => new(nameof(joinRequestId), joinRequestId);
    public static RouteReplace CashgameId(string cashgameId) => new(nameof(cashgameId), cashgameId);
    public static RouteReplace Code(string code) => new(nameof(code), code);
    public static RouteReplace EventId(string eventId) => new(nameof(eventId), eventId);
    public static RouteReplace LocationId(string locationId) => new(nameof(locationId), locationId);
    public static RouteReplace PlayerId(string playerId) => new(nameof(playerId), playerId);
    public static RouteReplace UserName(string userName) => new(nameof(userName), userName);
    public static RouteReplace Year(int year) => new(nameof(year), year.ToString());
}

public static class RouteParams
{
    public static string Replace(string format, params RouteReplace[] routeReplaces) => 
        routeReplaces.Aggregate(format, (current, rp) => Replace(current, $"{{{rp.Pattern}}}", rp.Value));

    private static string Replace(string format, string key, string value) => format.Replace(key, value);
}