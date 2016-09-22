using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class RunningCashgameGameJsonUrl : SlugUrl
    {
        public RunningCashgameGameJsonUrl(string slug)
            : base(WebRoutes.Cashgame.RunningGameJson, slug)
        {
        }
    }
}