using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class ClearCacheUrl : SiteUrl
    {
        public ClearCacheUrl()
            : base(WebRoutes.Admin.ClearCache)
        {
        }
    }
}