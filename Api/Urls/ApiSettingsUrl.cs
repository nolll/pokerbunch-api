using Api.Endpoints.Routes;
using Api.Urls.ApiUrls;

namespace Api.Urls;

public class ApiSettingsUrl : ApiUrl
{
    protected override string Input => ApiRoutes.Settings;
}