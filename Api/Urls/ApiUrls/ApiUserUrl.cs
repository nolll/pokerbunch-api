using Api.Routes;

namespace Api.Urls.ApiUrls
{
    public class ApiUserUrl : ApiUrl
    {
        private readonly string _userName;

        public ApiUserUrl(string userName)
        {
            _userName = userName;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.UserGet, RouteParam.UserName(_userName));
    }
}