using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls
{
    public class ApiUserUrl : ApiUrl
    {
        private readonly string _userName;

        public ApiUserUrl(string host, string userName) : base(host)
        {
            _userName = userName;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.User.Get, RouteReplace.UserName(_userName));
    }
}