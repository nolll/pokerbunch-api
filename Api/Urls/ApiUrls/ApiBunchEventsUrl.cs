using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiBunchEventsUrl : ApiUrl
    {
        private readonly string _bunchId;

        public ApiBunchEventsUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Event.ListByBunch, RouteReplace.BunchId(_bunchId));
    }
}