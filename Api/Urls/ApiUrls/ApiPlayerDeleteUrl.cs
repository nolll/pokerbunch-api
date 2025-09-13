using Api.Endpoints.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiPlayerDeleteUrl(string playerId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.Player.Delete, RouteReplace.PlayerId(playerId));
}