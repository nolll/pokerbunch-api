using PokerBunch.Common.Routes;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiAdminClearCacheUrl : ApiUrl
    {
        protected override string Input => ApiRoutes.Admin.ClearCache;
    }
}