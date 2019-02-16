using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiAppUrl : ApiUrl
    {
        private readonly string _appId;
        
        public ApiAppUrl(string appId)
        {
            _appId = appId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.App.Get, RouteReplace.AppId(_appId));
    }
}