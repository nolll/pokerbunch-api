using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiPlayerListUrl(string bunchId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.Player.ListByBunch, RouteReplace.BunchId(bunchId));
}