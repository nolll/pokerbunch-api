using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiBunchCashgamesUrl : ApiUrl
    {
        private readonly string _bunchId;
        private readonly int? _year;

        public ApiBunchCashgamesUrl(string bunchId, int? year = null)
        {
            _bunchId = bunchId;
            _year = year;
        }

        protected override string Input
        {
            get
            {
                if(_year.HasValue)
                    return RouteParams.Replace(ApiRoutes.Cashgame.ListByBunchAndYear, RouteReplace.BunchId(_bunchId), RouteReplace.Year(_year.Value));
                return RouteParams.Replace(ApiRoutes.Cashgame.ListByBunch, RouteReplace.BunchId(_bunchId));
            }
        }
    }
}