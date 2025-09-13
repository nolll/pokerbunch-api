using Api.Endpoints.Routes;

namespace Api.Urls.ApiUrls;

public class ApiUserListUrl : ApiUrl
{
    protected override string Input => ApiRoutes.User.List;
}