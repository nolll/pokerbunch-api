using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class AddBunchConfirmationUrl : SiteUrl
    {
        public AddBunchConfirmationUrl()
            : base(WebRoutes.Bunch.AddConfirmation)
        {
        }
    }
}