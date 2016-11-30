using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class ChartUrl : BunchWithOptionalYearUrl
    {
        public ChartUrl(string slug, int? year)
            : base(WebRoutes.Cashgame.Chart, WebRoutes.Cashgame.ChartWithYear, slug, year)
        {
        }
    }
}