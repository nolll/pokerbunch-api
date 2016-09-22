using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class EventListUrl : SlugUrl
    {
        public EventListUrl(string slug)
            : base(WebRoutes.Event.List, slug)
        {
        }
    }
}