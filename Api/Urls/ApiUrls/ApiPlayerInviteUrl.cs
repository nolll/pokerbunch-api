using Api.Endpoints.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiPlayerInviteUrl(string playerId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.Player.Invite, RouteReplace.PlayerId(playerId));
}