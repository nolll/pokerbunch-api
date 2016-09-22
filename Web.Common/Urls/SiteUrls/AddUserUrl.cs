using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class AddUserUrl : SiteUrl
    {
        public AddUserUrl()
            : base(WebRoutes.User.Add)
        {
        }
    }
}