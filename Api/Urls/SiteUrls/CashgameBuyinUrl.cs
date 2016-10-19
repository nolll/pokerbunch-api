using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class CashgameBuyinUrl : SlugUrl
    {
        public CashgameBuyinUrl(string slug)
            : base(WebRoutes.Cashgame.Buyin, slug)
        {
        }
    }
}