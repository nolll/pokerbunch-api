using Api.Endpoints.Routes;

namespace Api.Urls.ApiUrls;

public class ApiVersionUrl : ApiUrl
{
    protected override string Input => ApiRoutes.Version;
}