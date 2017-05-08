using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls
{
    public class ApiUserUrl : ApiUrl
    {
        private readonly string _userName;

        public ApiUserUrl(string userName)
        {
            _userName = userName;
        }

        protected override string Input => RouteParams.ReplaceUserName(ApiRoutes.UserGet, _userName);
    }
}