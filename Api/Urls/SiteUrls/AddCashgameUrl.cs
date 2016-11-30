using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class AddCashgameUrl : SlugUrl
    {
        public AddCashgameUrl(string slug)
            : base(WebRoutes.Cashgame.Add, slug)
        {
        }
    }
}