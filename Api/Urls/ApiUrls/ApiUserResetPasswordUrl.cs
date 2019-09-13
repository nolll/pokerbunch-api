using Api.Routes;

namespace Api.Urls.ApiUrls
{
    public class ApiUserResetPasswordUrl : ApiUrl
    {
        protected override string Input => ApiRoutes.Profile.PasswordReset;

        public ApiUserResetPasswordUrl(string host) : base(host)
        {
        }
    }
}