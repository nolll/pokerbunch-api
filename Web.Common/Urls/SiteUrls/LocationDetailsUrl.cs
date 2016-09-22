using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class LocationDetailsUrl : IdUrl
    {
        public LocationDetailsUrl(int locationId)
            : base(WebRoutes.Location.Details, locationId)
        {
        }
    }
}