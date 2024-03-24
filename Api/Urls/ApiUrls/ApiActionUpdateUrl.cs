using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiActionUpdateUrl(string cashgameId, string actionId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.Action.Update, RouteReplace.CashgameId(cashgameId), RouteReplace.ActionId(actionId));
}