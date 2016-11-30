using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class DeletePlayerUrl : IdUrl
    {
        public DeletePlayerUrl(int playerId)
            : base(WebRoutes.Player.Delete, playerId)
        {
        }
    }
}