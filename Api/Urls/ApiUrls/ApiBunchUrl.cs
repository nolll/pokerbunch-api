using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiBunchUrl : ApiUrl
    {
        private readonly string _bunchId;

        public ApiBunchUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Bunch.Get, RouteReplace.BunchId(_bunchId));
    }
}