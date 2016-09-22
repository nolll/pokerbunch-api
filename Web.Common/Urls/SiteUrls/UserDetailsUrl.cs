using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class UserDetailsUrl : UserUrl
    {
        public UserDetailsUrl(string userName)
            : base(WebRoutes.User.Details, userName)
        {
        }
    }
}