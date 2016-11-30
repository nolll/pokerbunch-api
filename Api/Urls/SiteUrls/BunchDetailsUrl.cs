using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class BunchDetailsUrl : SlugUrl
    {
        public BunchDetailsUrl(string slug)
            : base(WebRoutes.Bunch.Details, slug)
        {
        }
    }
}