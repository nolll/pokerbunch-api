using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class UserAppsUrl : SiteUrl
    {
        public UserAppsUrl()
            : base(WebRoutes.App.List)
        {
        }
    }
}