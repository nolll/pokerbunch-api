using PokerBunch.Common.Routes;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiUserBunchesUrl : ApiUrl
    {
        protected override string Input => ApiRoutes.Bunch.ListForCurrentUser;
    }
}