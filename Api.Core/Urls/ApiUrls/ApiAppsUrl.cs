using Api.Routes;

namespace Api.Urls.ApiUrls
{
    public class ApiAppsUrl : ApiUrl
    {
        protected override string Input => ApiRoutes.App.List;

        public ApiAppsUrl(string host) : base(host)
        {
        }
    }
}