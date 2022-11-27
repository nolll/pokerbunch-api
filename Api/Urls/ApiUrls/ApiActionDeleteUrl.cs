using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiActionDeleteUrl : ApiUrl
{
    private readonly string _cashgameId;
    private readonly string _actionId;

    public ApiActionDeleteUrl(string cashgameId, string actionId)
    {
        _cashgameId = cashgameId;
        _actionId = actionId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Action.Delete, RouteReplace.CashgameId(_cashgameId), RouteReplace.ActionId(_actionId));
}