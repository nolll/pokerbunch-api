using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class PlayerIndexUrl : SlugUrl
    {
        public PlayerIndexUrl(string slug)
            : base(WebRoutes.Player.List, slug)
        {
        }
    }
}