using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class ForgotPasswordConfirmationUrl : SiteUrl
    {
        public ForgotPasswordConfirmationUrl()
            : base(WebRoutes.User.ForgotPasswordConfirmation)
        {
        }
    }
}