using Api.Endpoints.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiCashgameUpdateUrl(string cashgameId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.Update, RouteReplace.CashgameId(cashgameId));
}