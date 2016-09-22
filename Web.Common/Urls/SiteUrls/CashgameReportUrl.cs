using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class CashgameReportUrl : SlugUrl
    {
        public CashgameReportUrl(string slug)
            : base(WebRoutes.Cashgame.Report, slug)
        {
        }
    }
}