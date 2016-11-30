using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class RunningCashgamePlayersJsonUrl : SlugUrl
    {
        public RunningCashgamePlayersJsonUrl(string slug)
            : base(WebRoutes.Cashgame.RunningPlayersJson, slug)
        {
        }
    }
}