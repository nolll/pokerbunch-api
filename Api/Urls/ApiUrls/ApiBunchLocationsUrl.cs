using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls
{
    public class ApiBunchLocationsUrl : ApiUrl
    {
        private readonly string _bunchId;

        public ApiBunchLocationsUrl(string host, string bunchId) : base(host)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Location.ListByBunch, RouteReplace.BunchId(_bunchId));
    }
}