using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class AddEventUrl : SlugUrl
    {
        public AddEventUrl(string slug)
            : base(WebRoutes.Event.Add, slug)
        {
        }
    }
}