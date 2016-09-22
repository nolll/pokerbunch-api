using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class EditBunchUrl : SlugUrl
    {
        public EditBunchUrl(string slug)
            : base(WebRoutes.Bunch.Edit, slug)
        {
        }
    }
}