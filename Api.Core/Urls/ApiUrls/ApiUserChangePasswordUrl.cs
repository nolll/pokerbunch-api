using Api.Routes;

namespace Api.Urls.ApiUrls
{
    public class ApiUserChangePasswordUrl : ApiUrl
    {
        protected override string Input => ApiRoutes.Profile.PasswordChange;

        public ApiUserChangePasswordUrl(string host) : base(host)
        {
        }
    }
}