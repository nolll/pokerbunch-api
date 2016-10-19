using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class EventDetailsUrl : IdUrl
    {
        public EventDetailsUrl(int eventId)
            : base(WebRoutes.Event.Details, eventId)
        {
        }
    }
}