namespace PokerBunch.Common.Urls.SiteUrls
{
    public class DashboardUrl : SiteUrl
    {
        private readonly string _bunchId;

        public DashboardUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "cashgame/dashboard/{bunchId}";
    }
}