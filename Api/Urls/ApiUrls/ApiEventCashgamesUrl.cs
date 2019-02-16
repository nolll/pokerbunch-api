using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiEventCashgamesUrl : ApiUrl
    {
        private readonly string _eventId;

        public ApiEventCashgamesUrl(string eventId)
        {
            _eventId = eventId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.ListByEvent, RouteReplace.EventId(_eventId));
    }
}