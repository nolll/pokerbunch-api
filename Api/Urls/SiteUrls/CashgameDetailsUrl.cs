using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class CashgameDetailsUrl : IdUrl
    {
        public CashgameDetailsUrl(int id)
            : base(WebRoutes.Cashgame.Details, id)
        {
        }
    }
}