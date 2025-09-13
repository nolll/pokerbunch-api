using Api.Endpoints.Routes;

namespace Api.Urls.ApiUrls;

public class ApiSwaggerUrl : ApiUrl
{
    protected override string Input => ApiRoutes.Swagger;
}