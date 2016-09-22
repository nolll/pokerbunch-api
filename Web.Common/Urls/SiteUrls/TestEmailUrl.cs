using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class TestEmailUrl : SiteUrl
    {
        public TestEmailUrl()
            : base(WebRoutes.Admin.SendEmail)
        {
        }
    }
}