using Api.Endpoints.Routes;

namespace Api.Urls.ApiUrls;

public class ApiRefreshUrl : ApiUrl
{
    protected override string Input => ApiRoutes.Auth.Refresh;
}