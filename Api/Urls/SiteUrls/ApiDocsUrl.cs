using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class ApiDocsUrl : SiteUrl
    {
        protected override string Input => WebRoutes.Api.Docs;
    }
}