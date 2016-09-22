using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class ForgotPasswordConfirmationUrl : SiteUrl
    {
        public ForgotPasswordConfirmationUrl()
            : base(WebRoutes.User.ForgotPasswordConfirmation)
        {
        }
    }
}