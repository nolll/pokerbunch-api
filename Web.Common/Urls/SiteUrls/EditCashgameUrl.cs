using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class EditCashgameUrl : IdUrl
    {
        public EditCashgameUrl(int id)
            : base(WebRoutes.Cashgame.Edit, id)
        {
        }
    }
}