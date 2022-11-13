using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiBunchLocationsUrl : ApiUrl
{
    private readonly string _bunchId;

    public ApiBunchLocationsUrl(string bunchId)
    {
        _bunchId = bunchId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Location.ListByBunch, RouteReplace.BunchId(_bunchId));
}