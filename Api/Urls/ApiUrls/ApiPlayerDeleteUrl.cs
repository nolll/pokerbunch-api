using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiPlayerDeleteUrl : ApiUrl
{
    private readonly string _playerId;

    public ApiPlayerDeleteUrl(string playerId)
    {
        _playerId = playerId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Player.Delete, RouteReplace.PlayerId(_playerId));
}