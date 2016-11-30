using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class EditBunchUrl : SlugUrl
    {
        public EditBunchUrl(string slug)
            : base(WebRoutes.Bunch.Edit, slug)
        {
        }
    }
}