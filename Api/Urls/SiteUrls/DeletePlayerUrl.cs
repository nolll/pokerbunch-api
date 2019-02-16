namespace PokerBunch.Common.Urls.SiteUrls
{
    public class DeletePlayerUrl : SiteUrl
    {
        private readonly string _playerId;

        public DeletePlayerUrl(string playerId)
        {
            _playerId = playerId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.PlayerId(_playerId));
        public const string Route = "player/delete/{playerId}";
    }
}