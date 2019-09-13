using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls
{
    public class ApiAppUrl : ApiUrl
    {
        private readonly string _appId;
        
        public ApiAppUrl(string host, string appId) : base(host)
        {
            _appId = appId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.App.Get, RouteReplace.AppId(_appId));
    }
}