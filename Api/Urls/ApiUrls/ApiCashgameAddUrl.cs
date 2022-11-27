using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiCashgameAddUrl : ApiUrl
{
    private readonly string _bunchId;

    public ApiCashgameAddUrl(string bunchId)
    {
        _bunchId = bunchId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.Add, RouteReplace.BunchId(_bunchId));
}