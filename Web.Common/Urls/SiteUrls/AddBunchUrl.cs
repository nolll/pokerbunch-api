using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class AddBunchUrl : SiteUrl
    {
        public AddBunchUrl()
            : base(WebRoutes.Bunch.Add)
        {
        }
    }
}