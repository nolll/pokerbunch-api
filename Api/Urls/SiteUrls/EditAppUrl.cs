using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class EditAppUrl : IdUrl
    {
        public EditAppUrl(int appId)
            : base(WebRoutes.App.Edit, appId)
        {
        }
    }
}