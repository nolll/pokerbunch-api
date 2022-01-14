using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiBunchUrl : ApiUrl
{
    private readonly string _bunchId;

    public ApiBunchUrl(string host, string bunchId) : base(host)
    {
        _bunchId = bunchId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Bunch.Get, RouteReplace.BunchId(_bunchId));
}