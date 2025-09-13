using Api.Endpoints.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiActionDeleteUrl(string cashgameId, string actionId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.Action.Delete, RouteReplace.CashgameId(cashgameId), RouteReplace.ActionId(actionId));
}