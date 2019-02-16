namespace PokerBunch.Common.Urls.SiteUrls
{
    public class EventDetailsUrl : SiteUrl
    {
        private readonly string _eventId;

        public EventDetailsUrl(string eventId)
        {
            _eventId = eventId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.EventId(_eventId));
        public const string Route = "event/details/{eventId}";
    }
}