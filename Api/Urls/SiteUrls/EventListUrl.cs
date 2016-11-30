using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class EventListUrl : SlugUrl
    {
        public EventListUrl(string slug)
            : base(WebRoutes.Event.List, slug)
        {
        }
    }
}