using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiPlayerListUrl : ApiUrl
{
    private readonly string _bunchId;

    public ApiPlayerListUrl(string bunchId)
    {
        _bunchId = bunchId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Player.ListByBunch, RouteReplace.BunchId(_bunchId));
}