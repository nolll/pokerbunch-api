using Api.Routes;

namespace Api.Urls.ApiUrls
{
    public class ApiRunningGameUrl : SlugApiUrl
    {
        public ApiRunningGameUrl(string slug)
            : base(ApiRoutes.CurrentGames, slug)
        {
        }
    }
}