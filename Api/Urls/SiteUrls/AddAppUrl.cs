using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class AddAppUrl : SiteUrl
    {
        public AddAppUrl()
            : base(WebRoutes.App.Add)
        {
        }
    }
}