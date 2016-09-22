using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class RunningCashgameUrl : SlugUrl
    {
        public RunningCashgameUrl(string slug)
            : base(WebRoutes.Cashgame.Running, slug)
        {
        }
    }
}