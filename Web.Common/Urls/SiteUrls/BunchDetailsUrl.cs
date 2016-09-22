using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class BunchDetailsUrl : SlugUrl
    {
        public BunchDetailsUrl(string slug)
            : base(WebRoutes.Bunch.Details, slug)
        {
        }
    }
}