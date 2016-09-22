using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class UserAppsUrl : SiteUrl
    {
        public UserAppsUrl()
            : base(WebRoutes.App.List)
        {
        }
    }
}