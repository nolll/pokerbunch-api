namespace PokerBunch.Common.Urls.SiteUrls
{
    public class CashgameDetailsUrl : SiteUrl
    {
        private readonly string _cashgameId;

        public CashgameDetailsUrl(string cashgameId)
        {
            _cashgameId = cashgameId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.CashgameId(_cashgameId));
        public const string Route = "cashgame/details/{cashgameId}";
    }
}