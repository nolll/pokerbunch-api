namespace PokerBunch.Common.Urls.SiteUrls
{
    public class LocationDetailsUrl : SiteUrl
    {
        private readonly string _locationId;

        public LocationDetailsUrl(string locationId)
        {
            _locationId = locationId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.LocationId(_locationId));
        public const string Route = "location/details/{locationId}";
    }
}