using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiBunchUpdateUrl : ApiUrl
{
    private readonly string _bunchId;

    public ApiBunchUpdateUrl(string bunchId)
    {
        _bunchId = bunchId;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Bunch.Update, RouteReplace.BunchId(_bunchId));
}