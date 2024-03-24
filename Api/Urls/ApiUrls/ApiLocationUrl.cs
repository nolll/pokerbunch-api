using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiLocationUrl(string locationId) : ApiUrl
{
    public ApiLocationUrl(int locationId)
        : this(locationId.ToString())
    {
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Location.Get, RouteReplace.LocationId(locationId));
}