using Api.Endpoints.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiEventCashgamesUrl(string eventId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.ListByEvent, RouteReplace.EventId(eventId));
}