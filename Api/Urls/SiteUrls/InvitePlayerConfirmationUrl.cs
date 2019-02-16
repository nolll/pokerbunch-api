namespace PokerBunch.Common.Urls.SiteUrls
{
    public class InvitePlayerConfirmationUrl : SiteUrl
    {
        private readonly string _playerId;

        public InvitePlayerConfirmationUrl(string playerId)
        {
            _playerId = playerId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.PlayerId(_playerId));
        public const string Route = "player/invited/{playerId}";
    }
}