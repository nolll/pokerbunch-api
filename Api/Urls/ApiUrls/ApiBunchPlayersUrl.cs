using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiBunchPlayersUrl : ApiUrl
{
    private readonly string _bunchId;

    public ApiBunchPlayersUrl(string host, string bunchId) : base(host)
    {
        _bunchId = bunchId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Player.ListByBunch, RouteReplace.BunchId(_bunchId));
}