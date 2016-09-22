using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class CashgameIndexUrl : SlugUrl
    {
        public CashgameIndexUrl(string slug)
            : base(WebRoutes.Cashgame.Index, slug)
        {
        }
    }
}