using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class JoinBunchConfirmationUrl : SlugUrl
    {
        public JoinBunchConfirmationUrl(string slug)
            : base(WebRoutes.Bunch.JoinConfirmation, slug)
        {
        }
    }
}