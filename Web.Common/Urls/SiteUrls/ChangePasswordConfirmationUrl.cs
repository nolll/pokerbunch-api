using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class ChangePasswordConfirmationUrl : SiteUrl
    {
        public ChangePasswordConfirmationUrl()
            : base(WebRoutes.User.ChangePasswordConfirmation)
        {
        }
    }
}