using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class AddPlayerConfirmationUrl : SlugUrl
    {
        public AddPlayerConfirmationUrl(string slug)
            : base(WebRoutes.Player.AddConfirmation, slug)
        {
        }
    }
}