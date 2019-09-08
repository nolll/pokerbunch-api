using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls
{
    public class ApiBunchEventsUrl : ApiUrl
    {
        private readonly string _bunchId;

        public ApiBunchEventsUrl(string host, string bunchId) : base(host)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Event.ListByBunch, RouteReplace.BunchId(_bunchId));
    }
}