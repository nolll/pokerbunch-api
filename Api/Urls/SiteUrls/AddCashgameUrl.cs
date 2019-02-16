namespace PokerBunch.Common.Urls.SiteUrls
{
    public class AddCashgameUrl : SiteUrl
    {
        private readonly string _bunchId;

        public AddCashgameUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "cashgame/add/{bunchId}";
    }
}