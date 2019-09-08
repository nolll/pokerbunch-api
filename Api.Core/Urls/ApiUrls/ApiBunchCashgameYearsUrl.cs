using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls
{
    public class ApiBunchCashgameYearsUrl : ApiUrl
    {
        private readonly string _bunchId;

        public ApiBunchCashgameYearsUrl(string host, string bunchId) : base(host)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.YearsByBunch, RouteReplace.BunchId(_bunchId));
    }
}