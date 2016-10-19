using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class TestEmailUrl : SiteUrl
    {
        public TestEmailUrl()
            : base(WebRoutes.Admin.SendEmail)
        {
        }
    }
}