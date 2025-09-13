using Api.Endpoints.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiPlayerCashgamesUrl(string playerId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.ListByPlayer, RouteReplace.PlayerId(playerId));
}