using PokerBunch.Common.Routes;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiUserProfileUrl : ApiUrl
    {
        protected override string Input => ApiRoutes.Profile.Get;
    }
}