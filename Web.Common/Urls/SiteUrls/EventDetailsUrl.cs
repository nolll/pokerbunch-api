using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class EventDetailsUrl : IdUrl
    {
        public EventDetailsUrl(int eventId)
            : base(WebRoutes.Event.Details, eventId)
        {
        }
    }
}