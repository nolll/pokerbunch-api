namespace PokerBunch.Common.Urls.SiteUrls
{
    public class ErrorForbiddenUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "error/forbidden";
    }
}