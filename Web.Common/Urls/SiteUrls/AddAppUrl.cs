using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class AddAppUrl : SiteUrl
    {
        public AddAppUrl()
            : base(WebRoutes.App.Add)
        {
        }
    }
}