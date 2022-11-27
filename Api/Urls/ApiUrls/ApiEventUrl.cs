using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiEventUrl : ApiUrl
{
    private readonly string _eventId;

    public ApiEventUrl(string eventId)
    {
        _eventId = eventId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Event.Get, RouteReplace.EventId(_eventId));
}