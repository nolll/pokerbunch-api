using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class LogoutUrl : SiteUrl
    {
        public LogoutUrl()
            : base(WebRoutes.Auth.Logout)
        {
        }
    }
}