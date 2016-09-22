using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class LoginUrl : SiteUrl
    {
        public LoginUrl()
            : base(WebRoutes.Auth.Login)
        {
        }
    }
}