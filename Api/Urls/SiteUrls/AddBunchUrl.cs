using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class AddBunchUrl : SiteUrl
    {
        public AddBunchUrl()
            : base(WebRoutes.Bunch.Add)
        {
        }
    }
}