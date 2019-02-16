using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiBunchCashgamesCurrentUrl : ApiUrl
    {
        private readonly string _bunchId;

        public ApiBunchCashgamesCurrentUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.ListCurrentByBunch, RouteReplace.BunchId(_bunchId));
    }
}