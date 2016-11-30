using Api.Routes;

namespace Api.Urls.ApiUrls
{
    public class ApiBuyinUrl : SlugApiUrl
    {
        public ApiBuyinUrl(string slug)
            : base(ApiRoutes.Buyin, slug)
        {
        }
    }
}