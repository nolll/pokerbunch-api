using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class CashgameReportUrl : SlugUrl
    {
        public CashgameReportUrl(string slug)
            : base(WebRoutes.Cashgame.Report, slug)
        {
        }
    }
}