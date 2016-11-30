using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class InvitePlayerConfirmationUrl : IdUrl
    {
        public InvitePlayerConfirmationUrl(int playerId)
            : base(WebRoutes.Player.InviteConfirmation, playerId)
        {
        }
    }
}