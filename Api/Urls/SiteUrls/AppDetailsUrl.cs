namespace PokerBunch.Common.Urls.SiteUrls
{
    public class AppDetailsUrl : SiteUrl
    {
        private readonly string _appId;

        public AppDetailsUrl(string appId)
        {
            _appId = appId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.AppId(_appId));
        public const string Route = "apps/details/{appId}";
    }
}