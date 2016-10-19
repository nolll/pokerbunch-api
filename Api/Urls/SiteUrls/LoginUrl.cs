using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class LoginUrl : SiteUrl
    {
        public LoginUrl()
            : base(WebRoutes.Auth.Login)
        {
        }
    }
}