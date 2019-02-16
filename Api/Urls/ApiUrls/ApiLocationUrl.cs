using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiLocationUrl : ApiUrl
    {
        private readonly string _locationId;

        public ApiLocationUrl(string locationId)
        {
            _locationId = locationId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Location.Get, RouteReplace.LocationId(_locationId));
    }
}