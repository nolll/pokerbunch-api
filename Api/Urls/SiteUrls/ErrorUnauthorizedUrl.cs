namespace PokerBunch.Common.Urls.SiteUrls
{
    public class ErrorUnauthorizedUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "error/unauthorized";
    }
}