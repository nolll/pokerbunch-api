namespace PokerBunch.Common.Urls.SiteUrls
{
    public class AddEventUrl : SiteUrl
    {
        private readonly string _bunchId;

        public AddEventUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "event/add/{bunchId}";
    }
}