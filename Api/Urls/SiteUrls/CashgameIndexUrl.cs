namespace PokerBunch.Common.Urls.SiteUrls
{
    public class CashgameIndexUrl : SiteUrl
    {
        private readonly string _bunchId;

        public CashgameIndexUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "cashgame/index/{bunchId}";
    }
}