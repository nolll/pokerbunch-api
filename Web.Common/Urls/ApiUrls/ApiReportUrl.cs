using Web.Common.Routes;

namespace Web.Common.Urls.ApiUrls
{
    public class ApiReportUrl : SlugApiUrl
    {
        public ApiReportUrl(string slug)
            : base(ApiRoutes.Report, slug)
        {
        }
    }
}