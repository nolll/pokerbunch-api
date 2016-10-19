using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class FactsUrl : BunchWithOptionalYearUrl
    {
        public FactsUrl(string slug, int? year)
            : base(WebRoutes.Cashgame.Facts, WebRoutes.Cashgame.FactsWithYear, slug, year)
        {
        }
    }
}