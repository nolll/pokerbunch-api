using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class AddLocationConfirmationUrl : SlugUrl
    {
        public AddLocationConfirmationUrl(string slug)
            : base(WebRoutes.Location.AddConfirmation, slug)
        {
        }
    }
}