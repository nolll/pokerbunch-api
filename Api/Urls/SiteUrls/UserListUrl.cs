using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class UserListUrl : SiteUrl
    {
        public UserListUrl()
            : base(WebRoutes.User.List)
        {
        }
    }
}