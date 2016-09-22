using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class UserListUrl : SiteUrl
    {
        public UserListUrl()
            : base(WebRoutes.User.List)
        {
        }
    }
}