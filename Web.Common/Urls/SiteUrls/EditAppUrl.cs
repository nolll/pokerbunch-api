using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class EditAppUrl : IdUrl
    {
        public EditAppUrl(int appId)
            : base(WebRoutes.App.Edit, appId)
        {
        }
    }
}