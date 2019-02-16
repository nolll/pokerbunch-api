namespace PokerBunch.Common.Urls.SiteUrls
{
    public class UserAppsUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "apps/list";
    }
}