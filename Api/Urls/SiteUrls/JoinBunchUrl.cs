using Api.Routes;

namespace Api.Urls.SiteUrls;

public class JoinBunchUrl : SiteUrl
{
    private readonly string _code;
    private readonly string _bunchId;

    public JoinBunchUrl(string host, string bunchId, string code = null) : base(host)
    {
        _bunchId = bunchId;
        _code = code;
    }

    protected override string Input => _code != null
        ? RouteParams.Replace(SiteRoutes.JoinBunchWithCode, RouteReplace.BunchId(_bunchId), RouteReplace.Code(_code))
        : RouteParams.Replace(SiteRoutes.JoinBunch, RouteReplace.BunchId(_bunchId));
}