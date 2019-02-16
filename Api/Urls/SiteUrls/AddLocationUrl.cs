namespace PokerBunch.Common.Urls.SiteUrls
{
    public class AddLocationUrl : SiteUrl
    {
        private readonly string _bunchId;

        public AddLocationUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "location/add/{bunchId}";
    }
}