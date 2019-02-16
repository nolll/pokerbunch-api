namespace PokerBunch.Common.Urls.SiteUrls
{
    public class ApiDocsIndexUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "apidocs";
    }
}