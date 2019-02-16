namespace PokerBunch.Common.Urls.SiteUrls
{
    public class EndCashgameUrl : SiteUrl
    {
        private readonly string _slug;

        public EndCashgameUrl(string slug)
        {
            _slug = slug;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_slug));
        public const string Route = "cashgame/end/{bunchId}";
    }
}