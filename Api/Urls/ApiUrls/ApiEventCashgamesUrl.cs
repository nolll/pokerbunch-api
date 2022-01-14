using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiEventCashgamesUrl : ApiUrl
{
    private readonly string _eventId;

    public ApiEventCashgamesUrl(string host, string eventId) : base(host)
    {
        _eventId = eventId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.ListByEvent, RouteReplace.EventId(_eventId));
}