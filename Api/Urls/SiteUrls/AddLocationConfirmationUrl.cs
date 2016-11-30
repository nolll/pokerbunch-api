using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class AddLocationConfirmationUrl : SlugUrl
    {
        public AddLocationConfirmationUrl(string slug)
            : base(WebRoutes.Location.AddConfirmation, slug)
        {
        }
    }
}