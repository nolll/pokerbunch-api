using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class ChangePasswordUrl : SiteUrl
    {
        public ChangePasswordUrl()
            : base(WebRoutes.User.ChangePassword)
        {
        }
    }
}