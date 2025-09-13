using Api.Endpoints.Routes;

namespace Api.Urls.ApiUrls;

public class ApiRootUrl : ApiUrl
{
    protected override string Input => ApiRoutes.Root;
}