using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiBunchLocationsUrl : ApiUrl
    {
        private readonly string _bunchId;

        public ApiBunchLocationsUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Location.ListByBunch, RouteReplace.BunchId(_bunchId));
    }
}