using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiPlayerAddUrl : ApiUrl
{
    private readonly string _bunchId;

    public ApiPlayerAddUrl(string bunchId)
    {
        _bunchId = bunchId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Player.AddToBunch, RouteReplace.BunchId(_bunchId));
}