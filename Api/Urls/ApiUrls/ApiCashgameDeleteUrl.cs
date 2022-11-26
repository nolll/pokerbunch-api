using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiCashgameDeleteUrl : ApiUrl
{
    private readonly string _cashgameId;

    public ApiCashgameDeleteUrl(string cashgameId)
    {
        _cashgameId = cashgameId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.Delete, RouteReplace.CashgameId(_cashgameId));
}