using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiLocationListUrl : ApiUrl
{
    private readonly string _bunchId;

    public ApiLocationListUrl(string bunchId)
    {
        _bunchId = bunchId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Location.ListByBunch, RouteReplace.BunchId(_bunchId));
}