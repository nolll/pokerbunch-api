using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class AddPlayerUrl : SlugUrl
    {
        public AddPlayerUrl(string slug)
            : base(WebRoutes.Player.Add, slug)
        {
        }
    }
}