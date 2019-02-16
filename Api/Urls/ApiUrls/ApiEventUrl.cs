using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiEventUrl : ApiUrl
    {
        private readonly string _eventId;

        public ApiEventUrl(string eventId)
        {
            _eventId = eventId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Event.Get, RouteReplace.EventId(_eventId));
    }
}