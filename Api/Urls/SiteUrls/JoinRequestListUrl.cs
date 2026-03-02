using Api.Routes;

namespace Api.Urls.SiteUrls;

public class JoinRequestListUrl(string bunchId) : SiteUrl
{
    protected override string Input => RouteParams.Replace(SiteRoutes.JoinRequestList, RouteReplace.BunchId(bunchId));
}