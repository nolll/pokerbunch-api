namespace PokerBunch.Common.Urls.SiteUrls
{
    public class AllAppsUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "apps/all";
    }
}