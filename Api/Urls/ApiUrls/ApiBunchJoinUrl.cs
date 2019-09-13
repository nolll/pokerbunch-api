using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls
{
    public class ApiBunchJoinUrl : ApiUrl
    {
        private readonly string _bunchId;

        public ApiBunchJoinUrl(string host, string bunchId) : base(host)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Bunch.Join, RouteReplace.BunchId(_bunchId));
    }
}