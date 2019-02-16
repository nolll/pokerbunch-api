namespace PokerBunch.Common.Urls.SiteUrls
{
    public class EditAppUrl : SiteUrl
    {
        private readonly string _appId;

        public EditAppUrl(string appId)
        {
            _appId = appId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.AppId(_appId));
        public const string Route = "apps/edit/{appId}";
    }
}