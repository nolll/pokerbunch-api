using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiPlayerInviteUrl : ApiUrl
    {
        private readonly string _playerId;

        public ApiPlayerInviteUrl(string playerId)
        {
            _playerId = playerId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Player.Invite, RouteReplace.PlayerId(_playerId));
    }
}