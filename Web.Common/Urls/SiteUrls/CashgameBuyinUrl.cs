using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class CashgameBuyinUrl : SlugUrl
    {
        public CashgameBuyinUrl(string slug)
            : base(WebRoutes.Cashgame.Buyin, slug)
        {
        }
    }
}