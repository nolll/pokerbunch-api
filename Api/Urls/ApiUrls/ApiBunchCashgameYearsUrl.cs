using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiBunchCashgameYearsUrl : ApiUrl
    {
        private readonly string _bunchId;

        public ApiBunchCashgameYearsUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.YearsByBunch, RouteReplace.BunchId(_bunchId));
    }
}