using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class DashboardCashgameUrl : SlugUrl
    {
        public DashboardCashgameUrl(string slug)
            : base(WebRoutes.Cashgame.Dashboard, slug)
        {
        }
    }
}