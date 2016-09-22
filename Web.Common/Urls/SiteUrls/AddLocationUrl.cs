using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class AddLocationUrl : SlugUrl
    {
        public AddLocationUrl(string slug)
            : base(WebRoutes.Location.Add, slug)
        {
        }
    }
}