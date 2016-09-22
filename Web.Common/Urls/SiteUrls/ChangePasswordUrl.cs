using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class ChangePasswordUrl : SiteUrl
    {
        public ChangePasswordUrl()
            : base(WebRoutes.User.ChangePassword)
        {
        }
    }
}