using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class ForgotPasswordUrl : SiteUrl
    {
        public ForgotPasswordUrl()
            : base(WebRoutes.User.ForgotPassword)
        {
        }
    }
}