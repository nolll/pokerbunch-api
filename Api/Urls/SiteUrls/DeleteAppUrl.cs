namespace PokerBunch.Common.Urls.SiteUrls
{
    public class DeleteAppUrl : SiteUrl
    {
        private readonly string _appId;

        public DeleteAppUrl(string appId)
        {
            _appId = appId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.AppId(_appId));
        public const string Route = "apps/delete/{appId}";
    }
}