using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class PlayerDetailsUrl : IdUrl
    {
        public PlayerDetailsUrl(int playerId)
            : base(WebRoutes.Player.Details, playerId)
        {
        }
    }
}