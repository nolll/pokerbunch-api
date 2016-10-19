using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class DeleteCashgameUrl : IdUrl
    {
        public DeleteCashgameUrl(int id)
            : base(WebRoutes.Cashgame.Delete, id)
        {
        }
    }
}