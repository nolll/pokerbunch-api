using Web.Common.Routes;

namespace Web.Common.Urls.ApiUrls
{
    public class ApiRunningGameUrl : SlugApiUrl
    {
        public ApiRunningGameUrl(string slug)
            : base(ApiRoutes.RunningGame, slug)
        {
        }
    }
}