using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class EditCashgameUrl : IdUrl
    {
        public EditCashgameUrl(int id)
            : base(WebRoutes.Cashgame.Edit, id)
        {
        }
    }
}