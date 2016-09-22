using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class ClearCacheUrl : SiteUrl
    {
        public ClearCacheUrl()
            : base(WebRoutes.Admin.ClearCache)
        {
        }
    }
}