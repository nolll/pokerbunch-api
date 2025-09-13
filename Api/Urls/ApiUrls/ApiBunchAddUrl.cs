using Api.Endpoints.Routes;

namespace Api.Urls.ApiUrls;

public class ApiBunchAddUrl : ApiUrl
{
    protected override string Input => ApiRoutes.Bunch.Add;
}