using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class UserDetailsUrl : UserUrl
    {
        public UserDetailsUrl(string userName)
            : base(WebRoutes.User.Details, userName)
        {
        }
    }
}