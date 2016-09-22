using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class CashgameActionUrl : SiteUrl
    {
        public CashgameActionUrl(int cashgameId, int playerId)
            : base(BuildUrl(WebRoutes.Cashgame.Action, cashgameId, playerId))
        {
        }

        private static string BuildUrl(string format, int cashgameId, int playerId)
        {
            var url = RouteParams.ReplaceCashgameId(format, cashgameId);
            return RouteParams.ReplacePlayerId(url, playerId);
        }
    }
}