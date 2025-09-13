using Api.Endpoints.Routes;

namespace Api.Urls.ApiUrls;

public class ApiLoginUrl : ApiUrl
{
    protected override string Input => ApiRoutes.Auth.Login;
}