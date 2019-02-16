namespace PokerBunch.Common.Urls.SiteUrls
{
    public class AddPlayerUrl : SiteUrl
    {
        private readonly string _bunchId;

        public AddPlayerUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "player/add/{bunchId}";
    }
}