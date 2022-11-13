using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiCashgameUrl : ApiUrl
{
    private readonly string _cashgameId;

    public ApiCashgameUrl(string cashgameId)
    {
        _cashgameId = cashgameId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.Get, RouteReplace.CashgameId(_cashgameId));
}