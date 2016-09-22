using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class PlayerDetailsUrl : IdUrl
    {
        public PlayerDetailsUrl(int playerId)
            : base(WebRoutes.Player.Details, playerId)
        {
        }
    }
}