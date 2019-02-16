namespace PokerBunch.Common.Urls.SiteUrls
{
    public class EditLocationUrl : SiteUrl
    {
        private readonly string _locationId;

        public EditLocationUrl(string locationId)
        {
            _locationId = locationId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.LocationId(_locationId));
        public const string Route = "location/edit/{locationId}";
    }
}