namespace PokerBunch.Common.Urls.SiteUrls
{
    public class PlayerDetailsUrl : SiteUrl
    {
        private readonly string _playerId;

        public PlayerDetailsUrl(string playerId)
        {
            _playerId = playerId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.PlayerId(_playerId));
        public const string Route = "player/details/{playerId}";
    }
}