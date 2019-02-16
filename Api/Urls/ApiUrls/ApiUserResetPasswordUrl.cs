using PokerBunch.Common.Routes;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiUserResetPasswordUrl : ApiUrl
    {
        protected override string Input => ApiRoutes.Profile.PasswordReset;
    }
}