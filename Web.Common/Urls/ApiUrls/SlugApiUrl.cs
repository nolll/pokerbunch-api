using Web.Common.Urls.SiteUrls;

namespace Web.Common.Urls.ApiUrls
{
    public abstract class SlugApiUrl : ApiUrl
    {
        protected SlugApiUrl(string format, string slug)
            : base(RouteParams.ReplaceSlug(format, slug))
        {
        }
    }
}