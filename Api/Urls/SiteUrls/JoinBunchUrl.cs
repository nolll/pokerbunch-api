using Api.Endpoints.Routes;

namespace Api.Urls.SiteUrls;

public class JoinBunchUrl(string bunchId, string? code = null) : SiteUrl
{
    protected override string Input => code != null
        ? RouteParams.Replace(SiteRoutes.JoinBunchWithCode, RouteReplace.BunchId(bunchId), RouteReplace.Code(code))
        : RouteParams.Replace(SiteRoutes.JoinBunch, RouteReplace.BunchId(bunchId));
}