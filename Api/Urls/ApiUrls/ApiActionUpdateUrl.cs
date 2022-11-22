using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiActionUpdateUrl : ApiUrl
{
    private readonly string _cashgameId;
    private readonly string _actionId;

    public ApiActionUpdateUrl(string cashgameId, string actionId)
    {
        _cashgameId = cashgameId;
        _actionId = actionId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Action.Update, RouteReplace.CashgameId(_cashgameId), RouteReplace.ActionId(_actionId));
}