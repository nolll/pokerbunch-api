using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class AddUserConfirmationUrl : SiteUrl
    {
        public AddUserConfirmationUrl()
            : base(WebRoutes.User.AddConfirmation)
        {
        }
    }
}