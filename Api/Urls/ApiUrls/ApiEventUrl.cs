using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiEventUrl : ApiUrl
{
    private readonly string _eventId;

    public ApiEventUrl(string host, string eventId) : base(host)
    {
        _eventId = eventId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Event.Get, RouteReplace.EventId(_eventId));
}