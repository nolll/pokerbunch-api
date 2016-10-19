using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class ForgotPasswordUrl : SiteUrl
    {
        public ForgotPasswordUrl()
            : base(WebRoutes.User.ForgotPassword)
        {
        }
    }
}