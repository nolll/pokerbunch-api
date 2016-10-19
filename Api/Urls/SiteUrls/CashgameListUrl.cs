using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class CashgameListUrl : BunchWithOptionalYearUrl
    {
        public CashgameListUrl(string slug, int? year)
            : base(WebRoutes.Cashgame.List, WebRoutes.Cashgame.ListWithYear, slug, year)
        {
        }
    }
}