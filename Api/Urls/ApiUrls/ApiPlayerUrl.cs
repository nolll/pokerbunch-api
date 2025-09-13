using Api.Endpoints.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiPlayerUrl(string playerId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.Player.Get, RouteReplace.PlayerId(playerId));
}