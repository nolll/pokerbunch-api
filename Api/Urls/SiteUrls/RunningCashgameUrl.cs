using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class RunningCashgameUrl : SlugUrl
    {
        public RunningCashgameUrl(string slug)
            : base(WebRoutes.Cashgame.Running, slug)
        {
        }
    }
}