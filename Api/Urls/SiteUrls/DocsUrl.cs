using Api.Routes;

namespace Api.Urls.SiteUrls;

public class DocsUrl : SiteUrl
{
    protected override string Input => SiteRoutes.ApiDocs;
}