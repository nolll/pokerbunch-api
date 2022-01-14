using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiCashgameActionsUrl : ApiUrl
{
    private readonly string _cashgameId;

    public ApiCashgameActionsUrl(string host, string cashgameId) : base(host)
    {
        _cashgameId = cashgameId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Action.List, RouteReplace.CashgameId(_cashgameId));
}