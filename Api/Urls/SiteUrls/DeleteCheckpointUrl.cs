namespace PokerBunch.Common.Urls.SiteUrls
{
    public class DeleteCheckpointUrl : SiteUrl
    {
        private readonly string _cashgameId;
        private readonly string _actionId;

        public DeleteCheckpointUrl(string cashgameId, string actionId)
        {
            _cashgameId = cashgameId;
            _actionId = actionId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.CashgameId(_cashgameId), RouteReplace.ActionId(_actionId));
        public const string Route = "cashgame/deletecheckpoint/{cashgameId}/{actionId}";
    }
}