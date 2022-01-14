using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiPlayerUrl : ApiUrl
{
    private readonly string _playerId;

    public ApiPlayerUrl(string host, string playerId) : base(host)
    {
        _playerId = playerId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Player.Get, RouteReplace.PlayerId(_playerId));
}