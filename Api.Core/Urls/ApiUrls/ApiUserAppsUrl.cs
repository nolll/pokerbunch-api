using Api.Routes;

namespace Api.Urls.ApiUrls
{
    public class ApiUserAppsUrl : ApiUrl
    {
        protected override string Input => ApiRoutes.App.ListForCurrentUser;

        public ApiUserAppsUrl(string host) : base(host)
        {
        }
    }
}