using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiBunchJoinUrl(string bunchId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.Bunch.Join, RouteReplace.BunchId(bunchId));
}