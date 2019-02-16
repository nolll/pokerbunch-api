using PokerBunch.Common.Routes;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiAppsUrl : ApiUrl
    {
        protected override string Input => ApiRoutes.App.List;
    }
}