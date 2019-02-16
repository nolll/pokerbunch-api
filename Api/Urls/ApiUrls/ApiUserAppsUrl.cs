using PokerBunch.Common.Routes;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiUserAppsUrl : ApiUrl
    {
        protected override string Input => ApiRoutes.App.ListForCurrentUser;
    }
}