using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class AddEventConfirmationUrl : SlugUrl
    {
        public AddEventConfirmationUrl(string slug)
            : base(WebRoutes.Event.AddConfirmation, slug)
        {
        }
    }
}