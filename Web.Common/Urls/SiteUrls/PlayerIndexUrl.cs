using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class PlayerIndexUrl : SlugUrl
    {
        public PlayerIndexUrl(string slug)
            : base(WebRoutes.Player.List, slug)
        {
        }
    }
}