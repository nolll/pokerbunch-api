using Web.Common.Routes;

namespace Web.Common.Urls.ApiUrls
{
    public class ApiBuyinUrl : SlugApiUrl
    {
        public ApiBuyinUrl(string slug)
            : base(ApiRoutes.Buyin, slug)
        {
        }
    }
}