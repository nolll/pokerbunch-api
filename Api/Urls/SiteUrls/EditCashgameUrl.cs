namespace PokerBunch.Common.Urls.SiteUrls
{
    public class EditCashgameUrl : SiteUrl
    {
        private readonly string _cashgameId;

        public EditCashgameUrl(string cashgameId)
        {
            _cashgameId = cashgameId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.CashgameId(_cashgameId));
        public const string Route = "cashgame/edit/{cashgameId}";
    }
}