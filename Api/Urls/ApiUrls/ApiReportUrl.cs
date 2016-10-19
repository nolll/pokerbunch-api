using Api.Routes;

namespace Api.Urls.ApiUrls
{
    public class ApiReportUrl : SlugApiUrl
    {
        public ApiReportUrl(string slug)
            : base(ApiRoutes.Report, slug)
        {
        }
    }
}