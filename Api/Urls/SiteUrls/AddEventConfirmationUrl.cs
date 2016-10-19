using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class AddEventConfirmationUrl : SlugUrl
    {
        public AddEventConfirmationUrl(string slug)
            : base(WebRoutes.Event.AddConfirmation, slug)
        {
        }
    }
}