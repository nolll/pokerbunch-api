using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiLocationUrl : ApiUrl
{
    private readonly string _locationId;

    public ApiLocationUrl(string host, string locationId) : base(host)
    {
        _locationId = locationId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Location.Get, RouteReplace.LocationId(_locationId));
}