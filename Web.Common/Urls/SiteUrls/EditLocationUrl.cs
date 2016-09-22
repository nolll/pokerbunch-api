using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class EditLocationUrl : IdUrl
    {
        public EditLocationUrl(int locationId)
            : base(WebRoutes.Location.Edit, locationId)
        {
        }
    }
}