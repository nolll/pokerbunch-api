namespace PokerBunch.Common.Urls.SiteUrls
{
    public class EventListUrl : SiteUrl
    {
        private readonly string _bunchId;

        public EventListUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "event/list/{bunchId}";
    }
}