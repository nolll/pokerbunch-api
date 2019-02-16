namespace PokerBunch.Common.Urls.SiteUrls
{
    public class InvitePlayerUrl : SiteUrl
    {
        private readonly string _playerId;

        public InvitePlayerUrl(string playerId)
        {
            _playerId = playerId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.PlayerId(_playerId));
        public const string Route = "player/invite/{playerId}";
    }
}