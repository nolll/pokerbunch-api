using Api.Endpoints.Routes;

namespace Api.Urls.ApiUrls;

public class ApiUserResetPasswordUrl : ApiUrl
{
    protected override string Input => ApiRoutes.Profile.ResetPassword;
}