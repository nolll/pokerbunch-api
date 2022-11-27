using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiCashgameUpdateUrl : ApiUrl
{
    private readonly string _cashgameId;

    public ApiCashgameUpdateUrl(string cashgameId)
    {
        _cashgameId = cashgameId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.Update, RouteReplace.CashgameId(_cashgameId));
}