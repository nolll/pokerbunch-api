using Api.Endpoints.Routes;

namespace Api.Urls.ApiUrls;

public class ApiUserChangePasswordUrl : ApiUrl
{
    protected override string Input => ApiRoutes.Profile.ChangePassword;
}