namespace PokerBunch.Common.Urls.SiteUrls
{
    public class ApiDocsAuthUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "apidocs/auth";
    }
}