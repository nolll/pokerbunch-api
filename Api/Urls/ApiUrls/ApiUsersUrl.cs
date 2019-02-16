using PokerBunch.Common.Routes;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiUsersUrl : ApiUrl
    {
        protected override string Input => ApiRoutes.User.List;
    }
}