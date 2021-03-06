using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiPlayerInviteUrl : ApiUrl
{
    private readonly string _playerId;

    public ApiPlayerInviteUrl(string host, string playerId) : base(host)
    {
        _playerId = playerId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Player.Invite, RouteReplace.PlayerId(_playerId));
}