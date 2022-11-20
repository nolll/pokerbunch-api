using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiEventAddUrl : ApiUrl
{
    private readonly string _bunchId;

    public ApiEventAddUrl(string bunchId)
    {
        _bunchId = bunchId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Event.Add, RouteReplace.BunchId(_bunchId));
}