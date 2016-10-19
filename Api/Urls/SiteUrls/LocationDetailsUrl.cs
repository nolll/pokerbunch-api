using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class LocationDetailsUrl : IdUrl
    {
        public LocationDetailsUrl(int locationId)
            : base(WebRoutes.Location.Details, locationId)
        {
        }
    }
}