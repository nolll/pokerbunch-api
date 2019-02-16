using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiPlayerCashgamesUrl : ApiUrl
    {
        private readonly string _playerId;

        public ApiPlayerCashgamesUrl(string playerId)
        {
            _playerId = playerId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.ListByPlayer, RouteReplace.PlayerId(_playerId));
    }
}