using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class AddUserUrl : SiteUrl
    {
        public AddUserUrl()
            : base(WebRoutes.User.Add)
        {
        }
    }
}