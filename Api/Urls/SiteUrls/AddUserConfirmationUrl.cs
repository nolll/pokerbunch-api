using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class AddUserConfirmationUrl : SiteUrl
    {
        public AddUserConfirmationUrl()
            : base(WebRoutes.User.AddConfirmation)
        {
        }
    }
}