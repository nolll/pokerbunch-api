using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class DashboardCashgameUrl : SlugUrl
    {
        public DashboardCashgameUrl(string slug)
            : base(WebRoutes.Cashgame.Dashboard, slug)
        {
        }
    }
}