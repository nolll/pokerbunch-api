namespace PokerBunch.Common.Urls.SiteUrls
{
    public class RunningCashgameUrl : SiteUrl
    {
        private readonly string _bunchId;

        public RunningCashgameUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "cashgame/running/{bunchId}";
    }
}