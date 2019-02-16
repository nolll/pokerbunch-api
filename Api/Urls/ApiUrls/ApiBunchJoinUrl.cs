using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiBunchJoinUrl : ApiUrl
    {
        private readonly string _bunchId;

        public ApiBunchJoinUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Bunch.Join, RouteReplace.BunchId(_bunchId));
    }
}