using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class AppDetailsUrl : IdUrl
    {
        public AppDetailsUrl(int appId)
            : base(WebRoutes.App.Details, appId)
        {
        }
    }
}