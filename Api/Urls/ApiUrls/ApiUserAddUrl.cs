using Api.Endpoints.Routes;

namespace Api.Urls.ApiUrls;

public class ApiUserAddUrl : ApiUrl
{
    protected override string Input => ApiRoutes.User.Add;
}