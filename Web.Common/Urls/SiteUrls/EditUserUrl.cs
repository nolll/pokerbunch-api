using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class EditUserUrl : UserUrl
    {
        public EditUserUrl(string userName)
            : base(WebRoutes.User.Edit, userName)
        {
        }
    }
}