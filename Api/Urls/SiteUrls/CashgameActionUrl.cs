namespace PokerBunch.Common.Urls.SiteUrls
{
    public class CashgameActionUrl : SiteUrl
    {
        private readonly string _cashgameId;
        private readonly string _playerId;

        public CashgameActionUrl(string cashgameId, string playerId)
        {
            _cashgameId = cashgameId;
            _playerId = playerId;
        }
        
        protected override string Input => RouteParams.Replace(Route, RouteReplace.CashgameId(_cashgameId), RouteReplace.PlayerId(_playerId));
        public const string Route = "cashgame/action/{cashgameId}/{playerId}";
    }
}