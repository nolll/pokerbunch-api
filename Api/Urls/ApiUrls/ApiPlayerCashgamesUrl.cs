using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiPlayerCashgamesUrl : ApiUrl
{
    private readonly string _playerId;

    public ApiPlayerCashgamesUrl(string playerId)
    {
        _playerId = playerId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.ListByPlayer, RouteReplace.PlayerId(_playerId));
}