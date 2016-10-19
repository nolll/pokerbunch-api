using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class EditLocationUrl : IdUrl
    {
        public EditLocationUrl(int locationId)
            : base(WebRoutes.Location.Edit, locationId)
        {
        }
    }
}