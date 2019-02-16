using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiBunchPlayersUrl : ApiUrl
    {
        private readonly string _bunchId;

        public ApiBunchPlayersUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Player.ListByBunch, RouteReplace.BunchId(_bunchId));
    }
}