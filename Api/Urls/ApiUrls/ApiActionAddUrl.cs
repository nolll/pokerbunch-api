using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiActionAddUrl : ApiUrl
{
    private readonly string _cashgameId;

    public ApiActionAddUrl(string cashgameId)
    {
        _cashgameId = cashgameId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Action.Add, RouteReplace.CashgameId(_cashgameId));
}