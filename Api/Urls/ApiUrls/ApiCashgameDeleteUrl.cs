using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiCashgameDeleteUrl(string cashgameId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.Delete, RouteReplace.CashgameId(cashgameId));
}