using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class FactsUrl : BunchWithOptionalYearUrl
    {
        public FactsUrl(string slug, int? year)
            : base(WebRoutes.Cashgame.Facts, WebRoutes.Cashgame.FactsWithYear, slug, year)
        {
        }
    }
}