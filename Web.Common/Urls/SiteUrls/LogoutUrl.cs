using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class LogoutUrl : SiteUrl
    {
        public LogoutUrl()
            : base(WebRoutes.Auth.Logout)
        {
        }
    }
}