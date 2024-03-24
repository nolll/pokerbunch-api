using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiEventAddUrl(string bunchId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.Event.Add, RouteReplace.BunchId(bunchId));
}