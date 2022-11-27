using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiLocationAddUrl : ApiUrl
{
    private readonly string _bunchId;

    public ApiLocationAddUrl(string bunchId)
    {
        _bunchId = bunchId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Location.Add, RouteReplace.BunchId(_bunchId));
}