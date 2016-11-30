using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class InvitePlayerUrl : IdUrl
    {
        public InvitePlayerUrl(int playerId)
            : base(WebRoutes.Player.Invite, playerId)
        {
        }
    }
}