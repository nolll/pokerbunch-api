namespace PokerBunch.Common.Urls.SiteUrls
{
    public class ClearCacheUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "admin/clearcache";
    }
}