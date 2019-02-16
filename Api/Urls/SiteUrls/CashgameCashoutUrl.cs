namespace PokerBunch.Common.Urls.SiteUrls
{
    public class CashgameCashoutUrl : SiteUrl
    {
        private readonly string _bunchId;

        public CashgameCashoutUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "cashgame/cashout/{bunchId}";
    }
}