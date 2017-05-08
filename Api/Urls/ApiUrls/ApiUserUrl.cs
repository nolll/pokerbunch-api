using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls
{
    public class ApiUserUrl : ApiUrl
    {
        public ApiUserUrl(string userName)
            : base(RouteParams.ReplaceUserName(ApiRoutes.UserGet, userName))
        {
        }
    }
}