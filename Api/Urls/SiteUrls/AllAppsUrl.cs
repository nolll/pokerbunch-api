using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class AllAppsUrl : SiteUrl
    {
        public AllAppsUrl()
            : base(WebRoutes.App.All)
        {
        }
    }
}