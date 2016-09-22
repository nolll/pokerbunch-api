using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class EndCashgameUrl : SlugUrl
    {
        public EndCashgameUrl(string slug)
            : base(WebRoutes.Cashgame.End, slug)
        {
        }
    }
}