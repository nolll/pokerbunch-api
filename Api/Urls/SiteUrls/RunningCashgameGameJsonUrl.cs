using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class RunningCashgameGameJsonUrl : SlugUrl
    {
        public RunningCashgameGameJsonUrl(string slug)
            : base(WebRoutes.Cashgame.RunningGameJson, slug)
        {
        }
    }
}