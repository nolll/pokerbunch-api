using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class InvitePlayerConfirmationUrl : IdUrl
    {
        public InvitePlayerConfirmationUrl(int playerId)
            : base(WebRoutes.Player.InviteConfirmation, playerId)
        {
        }
    }
}