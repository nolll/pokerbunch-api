using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class CashgameCashoutUrl : SlugUrl
    {
        public CashgameCashoutUrl(string slug)
            : base(WebRoutes.Cashgame.Cashout, slug)
        {
        }
    }
}