using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class ChangePasswordConfirmationUrl : SiteUrl
    {
        public ChangePasswordConfirmationUrl()
            : base(WebRoutes.User.ChangePasswordConfirmation)
        {
        }
    }
}