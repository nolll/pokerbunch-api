using Api.Routes;

namespace Api.Urls.ApiUrls;

public class ApiBunchesUrl : ApiUrl
{
    protected override string Input => ApiRoutes.Bunch.List;
}