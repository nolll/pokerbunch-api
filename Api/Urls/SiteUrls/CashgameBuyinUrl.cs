namespace PokerBunch.Common.Urls.SiteUrls
{
    public class CashgameBuyinUrl : SiteUrl
    {
        private readonly string _bunchId;

        public CashgameBuyinUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "cashgame/buyin/{bunchId}";
    }
}