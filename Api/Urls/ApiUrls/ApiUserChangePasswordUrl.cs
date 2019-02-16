using PokerBunch.Common.Routes;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiUserChangePasswordUrl : ApiUrl
    {
        protected override string Input => ApiRoutes.Profile.PasswordChange;
    }
}