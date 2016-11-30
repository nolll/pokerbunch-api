using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class EndCashgameUrl : SlugUrl
    {
        public EndCashgameUrl(string slug)
            : base(WebRoutes.Cashgame.End, slug)
        {
        }
    }
}