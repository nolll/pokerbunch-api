using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class CashgameIndexUrl : SlugUrl
    {
        public CashgameIndexUrl(string slug)
            : base(WebRoutes.Cashgame.Index, slug)
        {
        }
    }
}