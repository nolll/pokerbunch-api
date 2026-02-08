using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiActionAddUrl(string cashgameId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.Action.Add, RouteReplace.CashgameId(cashgameId));
}