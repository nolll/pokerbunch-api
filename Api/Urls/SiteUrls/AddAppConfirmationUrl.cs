using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class AddAppConfirmationUrl : SiteUrl
    {
        public AddAppConfirmationUrl()
            : base(WebRoutes.App.AddConfirmation)
        {
        }
    }
}