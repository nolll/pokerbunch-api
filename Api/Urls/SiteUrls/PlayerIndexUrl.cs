namespace PokerBunch.Common.Urls.SiteUrls
{
    public class PlayerIndexUrl : SiteUrl
    {
        private readonly string _bunchId;

        public PlayerIndexUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "player/list/{bunchId}";
    }
}