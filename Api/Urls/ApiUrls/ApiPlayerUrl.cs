using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiPlayerUrl : ApiUrl
    {
        private readonly string _playerId;

        public ApiPlayerUrl(string playerId)
        {
            _playerId = playerId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Player.Get, RouteReplace.PlayerId(_playerId));
    }
}