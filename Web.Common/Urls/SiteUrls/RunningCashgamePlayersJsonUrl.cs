using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class RunningCashgamePlayersJsonUrl : SlugUrl
    {
        public RunningCashgamePlayersJsonUrl(string slug)
            : base(WebRoutes.Cashgame.RunningPlayersJson, slug)
        {
        }
    }
}