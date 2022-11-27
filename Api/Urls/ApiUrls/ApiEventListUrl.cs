using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiEventListUrl : ApiUrl
{
    private readonly string _bunchId;

    public ApiEventListUrl(string bunchId)
    {
        _bunchId = bunchId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Event.ListByBunch, RouteReplace.BunchId(_bunchId));
}