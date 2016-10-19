using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class CashgameCashoutUrl : SlugUrl
    {
        public CashgameCashoutUrl(string slug)
            : base(WebRoutes.Cashgame.Cashout, slug)
        {
        }
    }
}