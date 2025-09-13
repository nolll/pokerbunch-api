using Api.Endpoints.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiEventUrl(string eventId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.Event.Get, RouteReplace.EventId(eventId));
}