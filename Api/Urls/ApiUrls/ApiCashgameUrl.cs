using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiCashgameUrl(string cashgameId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.Get, RouteReplace.CashgameId(cashgameId));
}