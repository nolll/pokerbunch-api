namespace PokerBunch.Common.Urls.SiteUrls
{
    public class RunningCashgameGameJsonUrl : SiteUrl
    {
        private readonly string _bunchId;

        public RunningCashgameGameJsonUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "cashgame/runninggamejson/{bunchId}";
    }
}