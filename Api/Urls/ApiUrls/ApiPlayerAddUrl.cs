using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiPlayerAddUrl(string bunchId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.Player.Add, RouteReplace.BunchId(bunchId));
}