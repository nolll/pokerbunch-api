using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class AddPlayerUrl : SlugUrl
    {
        public AddPlayerUrl(string slug)
            : base(WebRoutes.Player.Add, slug)
        {
        }
    }
}