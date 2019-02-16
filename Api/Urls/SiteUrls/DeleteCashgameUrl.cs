namespace PokerBunch.Common.Urls.SiteUrls
{
    public class DeleteCashgameUrl : SiteUrl
    {
        private readonly string _cashgameId;

        public DeleteCashgameUrl(string cashgameId)
        {
            _cashgameId = cashgameId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.CashgameId(_cashgameId));
        public const string Route = "cashgame/delete/{cashgameId}";
    }
}